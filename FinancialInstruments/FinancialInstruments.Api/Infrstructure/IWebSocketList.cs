using System.Net.WebSockets;

namespace FinancialInstruments.Api.Infrstructure
{
	public interface IWebSocketList
	{
		List<WebSocket> WebSockets { get; }
	}
}
