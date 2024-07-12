using FinancialInstruments.Api.Infrstructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;

namespace FinancialInstruments.Api.Controllers
{
	[Route("/ws")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class WebSocketController : ControllerBase
	{
		private readonly IWebSocketList _webSocketlist;
		public WebSocketController(IWebSocketList webSocketlist)
		{
			_webSocketlist = webSocketlist;
		}

		public async Task Get()
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
				_webSocketlist.WebSockets.Add(webSocket);
				var message = Encoding.UTF8.GetBytes("Connected...");
				await webSocket.SendAsync(message, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

				var buffer = new byte[1024 * 4];
				var receiveResult = await webSocket.ReceiveAsync(
					new ArraySegment<byte>(buffer), CancellationToken.None);
				
				while (!receiveResult.CloseStatus.HasValue)
				{
					receiveResult = await webSocket.ReceiveAsync(
						new ArraySegment<byte>(buffer), CancellationToken.None);
				}
				//await Echo(webSocket);
			}
			else
			{
				HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
		}

		private async Task Operate(byte[] buffer)
		{
			var request = JsonConvert.DeserializeObject<SubscriptionRequest>(Encoding.UTF8.GetString(buffer));
			if (request.EventName.Equals("subscribe", StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(request.Ticker))
			{

			}
		}
	}
}
