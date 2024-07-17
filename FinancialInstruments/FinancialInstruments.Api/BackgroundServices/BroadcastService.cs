
using FinancialInstruments.Api.Infrstructure;
using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Integration;
using FinancialInstruments.Logic.Cache;
using FinancialInstruments.Logic.Services;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;

namespace FinancialInstruments.Api.BackgroundServices
{
	public class BroadcastService : BackgroundService
	{
		private readonly ISubscribtionService _subscriptionService;
		private readonly IWebSocketClient _webSocketClient;
		private readonly IQuoteWSCache _quoteWSCache;
		private readonly List<string> _tickersSubscribed = new List<string>();
		public BroadcastService(ISubscribtionService subscriptionService, 
			IWebSocketClient webSocketClient, 
			IQuoteWSCache quoteWSCache) 
		{
			_subscriptionService = subscriptionService;
			_webSocketClient = webSocketClient;
			_quoteWSCache = quoteWSCache;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await Task.Delay(2000);
			_webSocketClient.Connect();
			while (!stoppingToken.IsCancellationRequested)
			{
				var subscriptions = (await _subscriptionService.GetAllSubscriptions()).ToArray();
				var quotes = await _quoteWSCache.GetAllQuotes();
				var quotesSerialized = quotes.ToDictionary(item => item.Key, item => JsonConvert.SerializeObject(item.Value));
				if (subscriptions.Any())
				{
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
				await Task.Delay(1000);
			}
		}
	}
}
