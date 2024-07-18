using FinancialInstruments.Api.Infrstructure;
using FinancialInstruments.Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Text;

namespace FinancialInstruments.Api.Controllers
{
	[Route("/ws")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class WebSocketController : ControllerBase
	{
		private readonly ISubscribtionService _subscribionService;
		public WebSocketController(ISubscribtionService subscribtionService)
		{
			_subscribionService = subscribtionService;
		}

		public async Task Get()
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
				
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
						Log.Error("Subscription failed");
						await webSocket.SendAsync(Encoding.UTF8.GetBytes("Subscription failed"), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
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
	}
}
