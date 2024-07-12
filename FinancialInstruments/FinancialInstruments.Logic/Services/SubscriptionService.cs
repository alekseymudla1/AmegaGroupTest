using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Services
{
	public class SubscriptionService : ISubscribtionService
	{
		private readonly List<Subscription> _subscriptions = new List<Subscription>();
		public SubscriptionService()
		{

		}

		public async Task Subscribe(WebSocket socket, string ticker)
		{
			_subscriptions.Add(new Subscription(socket, ticker));
		}

		public async Task<IEnumerable<WebSocket>> GetWebSocketsForTicker(string ticker)
		{
			return _subscriptions.Where(s => s.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase)).Select(s => s.WebSocket);
		}
	}
}
