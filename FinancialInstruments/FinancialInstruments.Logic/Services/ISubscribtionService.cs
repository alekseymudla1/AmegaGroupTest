using System.Net.WebSockets;

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
