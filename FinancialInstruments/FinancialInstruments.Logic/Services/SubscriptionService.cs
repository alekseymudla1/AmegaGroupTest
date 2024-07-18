using FinancialInstruments.Domain.Interfaces;
using System.Net.WebSockets;

namespace FinancialInstruments.Logic.Services
{
	public class SubscriptionService : ISubscribtionService
	{
		private readonly List<Subscription> _subscriptions = new List<Subscription>();
		private readonly IWebSocketClient _webSocketClient;
		public SubscriptionService(IWebSocketClient webSocketClient)
		{
			_webSocketClient = webSocketClient;
		}

		public async Task Subscribe(WebSocket socket, string ticker)
		{
			if (!_subscriptions.Exists(s => s.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase)))
			{
				await _webSocketClient.Subscribe(ticker);
			}
			_subscriptions.Add(new Subscription(socket, ticker));
		}

		public async Task<IEnumerable<WebSocket>> GetWebSocketsForTicker(string ticker)
		{
			return _subscriptions.Where(s => s.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase)).Select(s => s.WebSocket);
		}

		public async Task<IEnumerable<Subscription>> GetAllSubscriptions()
		{
			return _subscriptions;
		}

		public async Task<IEnumerable<string>> GetAllTickers()
		{
			return _subscriptions.Select(sub => sub.Ticker).Distinct();
		}
	}
}
