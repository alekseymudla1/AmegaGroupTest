using FinancialInstruments.Api.BackgroundServices;
using FinancialInstruments.Api.Infrstructure;
using FinancialInstruments.Api.Middlewares;
using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;
using FinancialInstruments.Integration;
using FinancialInstruments.Integration.REST;
using FinancialInstruments.Logic.Cache;
using FinancialInstruments.Logic.Services;
using System.Text.Json.Serialization;

namespace FinancialInstruments.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			
			var token = builder.Configuration["Token"].ToString();
			builder.Services.Configure<ClientTimeout>(
				builder.Configuration.GetSection(ClientTimeout.Section));

			builder.Services.AddHostedService<BroadcastService>();

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSingleton<IQuoteSources, QuoteSources>();
			builder.Services.AddTransient<IQuoteRestClient, QuoteRestClient>();
			builder.Services.AddSingleton<IQuoteCache, QuoteCache>();
			builder.Services.AddTransient<IQuoteService, QuoteService>();
			builder.Services.AddTransient<IClientFactory, ClientFactory>();
			builder.Services.AddTransient<IForexClient, ForexClient>(services => 
				new ForexClient(token));
			builder.Services.AddTransient<ICryptoClient, CryptoClient>(services =>
				new CryptoClient(token));

			builder.Services.AddSingleton<IWebSocketList, WebSocketList>();
			builder.Services.AddSingleton<ISubscribtionService, SubscriptionService>();

			var app = builder.Build();
			var webSocketOptions = new WebSocketOptions
			{
				KeepAliveInterval = TimeSpan.FromMinutes(2)
			};

			app.UseWebSockets(webSocketOptions);
			//app.Use(async (context, next) =>
			//{
			//	if (context.Request.Path == "/ws")
			//	{
			//		if (context.WebSockets.IsWebSocketRequest)
			//		{
			//			using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
			//			//await Echo(webSocket);
			//		}
			//		else
			//		{
			//			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			//		}
			//	}
			//	else
			//	{
			//		await next(context);
			//	}

			//});

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
		//public static void Main(string[] args)
		//{
		//	var builder = WebApplication.CreateBuilder(args);

		//	builder.Services.AddControllers();

		//	builder.Services.AddEndpointsApiExplorer();

		//	builder.Services.AddSwaggerGen();

		//	/*builder.Services.ConfigureHttpJsonOptions(options =>
		//	{
		//		options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
		//	});*/

		//	var app = builder.Build();
		//	if (app.Environment.IsDevelopment())
		//	{
		//		app.UseSwagger();
		//		app.UseSwaggerUI();
		//	}

		//	app.UseHttpsRedirection();

		//	app.UseAuthorization();

		//	app.Run();
		//}
	}

	public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

	/*[JsonSerializable(typeof(Todo[]))]
	[JsonSerializable(typeof(List<string>))]
	[JsonSerializable(typeof(string[]))]
	[JsonSerializable(typeof(Instruments))]
	internal partial class AppJsonSerializerContext : JsonSerializerContext
	{

	}*/
}
