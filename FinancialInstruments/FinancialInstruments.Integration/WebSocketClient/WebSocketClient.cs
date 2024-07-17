using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;
using FinancialInstruments.Logic.Cache;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace FinancialInstruments.Integration.WebSocketClient
{
	public class WebSocketClient : IWebSocketClient
	{
		private const string SUBSCRIPTION_EVENT_NAME = "subscribe";
		private readonly IQuoteSources _quoteSources;
		private readonly IQuoteWSCache _quoteWSCache;
		private readonly string _token;
		private readonly Dictionary<Source, WebSocket> _webSocketClients = new Dictionary<Source, WebSocket>();

		public WebSocketClient(IQuoteSources quoteSources, IQuoteWSCache quoteWSCache, string token)
		{
			_quoteSources = quoteSources;
			_quoteWSCache = quoteWSCache;
			_token = token;
		}

		public async Task Connect()
		{
			await Connect(Source.Forex);
			await Connect(Source.Crypto);
		}

		public async Task Subscribe(string ticker)
		{
			var source = await _quoteSources.GetSource(ticker);
			var message = JsonConvert.SerializeObject(new
			{
				eventName = SUBSCRIPTION_EVENT_NAME,
				authorization = _token,
				eventData = new { tickers = new List<string>() { ticker } }
			}, Formatting.Indented);

			Console.WriteLine(message);
			await _webSocketClients[source].SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Binary, true, CancellationToken.None);
		}

		private async Task Connect(Source source)
		{
			var webSocketUri = GetWSUriBySource(source);
			try
			{	
				var webSocket = new ClientWebSocket();
				await webSocket.ConnectAsync(webSocketUri, CancellationToken.None);
				Console.WriteLine($@"Connected to {webSocketUri}");
				_webSocketClients.Add(source, webSocket);
				var receiveTask = Task.Run(async () =>
				{
					var buffer = new byte[1024];
					while (true)
					{
						var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

						if (result.MessageType == WebSocketMessageType.Close) break;

						try
						{
							var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
							Console.WriteLine($@"Received: {message}");
							if (TryDeserializeMessage(message, out var quote))
							{
								_quoteWSCache.SaveQuote(quote.Ticker, quote);
							}
							
						}
						catch (Exception e)
						{
							Console.WriteLine($@"Error: {e}");
						}
					}
				});
			}
			catch(Exception ex)
			{
				Console.WriteLine($@"Failed to connect to {webSocketUri}. Ex: {ex.Message}");
			}
		}

		private Uri GetWSUriBySource(Source source)
		{
			return source switch
			{
				Source.Forex => Constants.TiingoForexWSUrl,
				Source.Crypto => Constants.TiingoCryptoWSUrl,
				_ => throw new ArgumentOutOfRangeException(nameof(source), $@"Not expected source: {source}")
			};
		}

		private bool TryDeserializeMessage(string message, out Quote quote)
		{
			try
			{
				var response = JsonConvert.DeserializeObject<WSResponse>(message);
				if (response is { MessageType: "A" })
				{
					quote = response.ToQuote();
					return true;
				}
				quote = null;
				return false;
			}
			catch (Exception ex)
			{
				quote = null;
				return false;
			}
		}
	}
}
