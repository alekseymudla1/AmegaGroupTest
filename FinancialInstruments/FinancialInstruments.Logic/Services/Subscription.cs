using System.Net.WebSockets;

namespace FinancialInstruments.Logic.Services
{
	public record Subscription(WebSocket WebSocket, string Ticker);
}
