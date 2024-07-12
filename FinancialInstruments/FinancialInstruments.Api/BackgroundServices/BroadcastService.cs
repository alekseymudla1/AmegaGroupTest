
using FinancialInstruments.Api.Infrstructure;
using System.Text;

namespace FinancialInstruments.Api.BackgroundServices
{
	public class BroadcastService : BackgroundService
	{
		private readonly IWebSocketList _websocketList;
		public BroadcastService(IWebSocketList webSocketList) 
		{ 
			_websocketList = webSocketList;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				Parallel.ForEach(_websocketList.WebSockets, ws => ws.SendAsync(Encoding.UTF8.GetBytes($@"Now is {DateTime.Now.ToString("HH:mm:ss")}"), System.Net.WebSockets.WebSocketMessageType.Text, true, stoppingToken));

				await Task.Delay(1000);
			}
		}
	}
}
