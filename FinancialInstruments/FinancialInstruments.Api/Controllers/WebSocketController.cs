using FinancialInstruments.Api.Infrstructure;
using FinancialInstruments.Logic.Services;
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
		private readonly ISubscribtionService _subscribionService;
		public WebSocketController(IWebSocketList webSocketlist, ISubscribtionService subscribtionService)
		{
			_webSocketlist = webSocketlist;
			_subscribionService = subscribtionService;
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
					var data = Encoding.UTF8.GetString(buffer);
					try
					{
						var request = JsonConvert.DeserializeObject<SubscriptionRequest>(data);
						if (request is { EventName: "subscribe" })
						{
							await _subscribionService.Subscribe(webSocket, request.Ticker);
							await webSocket.SendAsync(Encoding.UTF8.GetBytes("Subscription succeded"), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
						}
					}
					catch(Exception)
					{
						Console.WriteLine("Subscription failed");
						await webSocket.SendAsync(Encoding.UTF8.GetBytes("Request is failed"), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
					}
					finally
					{
						receiveResult = await webSocket.ReceiveAsync(
							new ArraySegment<byte>(buffer), CancellationToken.None);
					}
				}
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
