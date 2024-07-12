using System.Net.WebSockets;

namespace FinancialInstruments.Api.Infrstructure
{
	public class WebSocketList : IWebSocketList
	{
		private readonly List<WebSocket> _list = new List<WebSocket>();
		public WebSocketList()
		{

		}
		public List<WebSocket> WebSockets => _list;
	}
}
