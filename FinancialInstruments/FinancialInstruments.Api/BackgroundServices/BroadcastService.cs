using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;
using FinancialInstruments.Logic.Cache;
using FinancialInstruments.Logic.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace FinancialInstruments.Api.BackgroundServices
{
	public class BroadcastService : BackgroundService
	{
		private const int START_DELAY = 2000;
		private readonly ISubscribtionService _subscriptionService;
		private readonly IWebSocketClient _webSocketClient;
		private readonly IQuoteWSCache _quoteWSCache;
		private readonly List<string> _tickersSubscribed = new List<string>();
		private readonly int _interval;

		private readonly IHostApplicationLifetime _hostApplicationLifetime;

		public BroadcastService(ISubscribtionService subscriptionService,
			IWebSocketClient webSocketClient,
			IQuoteWSCache quoteWSCache,
			IHostApplicationLifetime hostApplicationLifetime,
			IOptions<BroadcastOptions> broadcastOptions
			)
		{
			_subscriptionService = subscriptionService;
			_webSocketClient = webSocketClient;
			_quoteWSCache = quoteWSCache;
			_hostApplicationLifetime = hostApplicationLifetime;
			_interval = broadcastOptions?.Value?.Interval ?? 1000;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{


			await base.StartAsync(cancellationToken);
		}


		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_hostApplicationLifetime.ApplicationStopping.Register(OnStopping);

			await Task.Delay(START_DELAY); // Just for correct start
			_webSocketClient.Connect(); // we have no need to wait this

			while (!stoppingToken.IsCancellationRequested)
			{
				var subscriptions = (await _subscriptionService.GetAllSubscriptions()).ToArray();
				var quotes = await _quoteWSCache.GetAllQuotes();
				var quotesSerialized = quotes.ToDictionary(item => item.Key, item => JsonConvert.SerializeObject(item.Value));
				if (subscriptions.Any())
				{
					// We have parallel tasks with small body, so I used
					// https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-speed-up-small-loop-bodies
					// *************************************************
					var rangePartitioner = Partitioner.Create(0, subscriptions.Count());
					Parallel.ForEach(rangePartitioner, (range, loopState) =>
					{
						// Loop over each range element without a delegate invocation.
						for (int i = range.Item1; i < range.Item2; i++)
						{
							if (quotes.ContainsKey(subscriptions[i].Ticker))
							{
								subscriptions[i].WebSocket.SendAsync(Encoding.UTF8.GetBytes(quotesSerialized[subscriptions[i].Ticker]), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
							}
						}
					});
				}
				await Task.Delay(_interval);
			}

			if (stoppingToken.IsCancellationRequested)
			{
				Log.Information("Finish service");
			}
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			Log.Warning("BroadcastService STOPPING: {time}", DateTimeOffset.Now);
			await base.StopAsync(cancellationToken);
		}

		private void OnStopping()
		{
			Log.Warning("BroadcastService STOPPING: {time}", DateTimeOffset.Now);
			var subscriptions = _subscriptionService.GetAllSubscriptions().Result.ToArray();
			foreach (var webSocket in subscriptions.Select(sub => sub.WebSocket).Distinct())
			{
				webSocket.CloseOutputAsync(WebSocketCloseStatus.EndpointUnavailable,
					"The service is shutting down",
					CancellationToken.None);
			}
		}

	}
}
