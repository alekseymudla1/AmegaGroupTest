using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Services
{
	public interface ISubscribtionService
	{
		Task Subscribe(WebSocket socket, string ticker);

		Task<IEnumerable<WebSocket>> GetWebSocketsForTicker(string ticker);

		Task<IEnumerable<Subscription>> GetAllSubscriptions();

		Task<IEnumerable<string>> GetAllTickers();
	}
}
