using FinancialInstruments.Api.BackgroundServices;
using FinancialInstruments.Api.Middlewares;
using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;
using FinancialInstruments.Integration;
using FinancialInstruments.Integration.REST;
using FinancialInstruments.Integration.WebSocketClient;
using FinancialInstruments.Logic.Cache;
using FinancialInstruments.Logic.Services;
using Serilog;

namespace FinancialInstruments.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Host.UseSerilog((context, configuration) =>
				configuration.ReadFrom.Configuration(context.Configuration));

			// Settings
			var token = builder.Configuration["Token"].ToString();
			builder.Services.Configure<ClientTimeoutOptions>(
				builder.Configuration.GetSection(ClientTimeoutOptions.Section));
			builder.Services.Configure<BroadcastOptions>(
				builder.Configuration.GetSection(BroadcastOptions.Section));

			// Hosting service
			builder.Services.AddHostedService<BroadcastService>();

			// Controllers
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// REST-part dependencies
			builder.Services.AddSingleton<IQuoteSources, QuoteSources>();
			builder.Services.AddTransient<IQuoteRestClient, QuoteRestClient>();
			builder.Services.AddSingleton<IQuoteCache, QuoteCache>();
			builder.Services.AddTransient<IQuoteService, QuoteService>();
			builder.Services.AddTransient<IClientFactory, ClientFactory>();
			builder.Services.AddTransient<IForexClient, ForexClient>(services =>
				new ForexClient(token));
			builder.Services.AddTransient<ICryptoClient, CryptoClient>(services =>
				new CryptoClient(token));

			// WebSocket-part DI
			builder.Services.AddSingleton<ISubscribtionService, SubscriptionService>();
			builder.Services.AddSingleton<IQuoteWSCache, QuoteWSCache>();
			builder.Services.AddSingleton<IWebSocketClient>(services =>
				new WebSocketClient(
					services.GetRequiredService<IQuoteSources>(),
					services.GetRequiredService<IQuoteWSCache>(),
					token));

			// Building the app
			var app = builder.Build();
			var webSocketOptions = new WebSocketOptions
			{
				KeepAliveInterval = TimeSpan.FromMinutes(2)
			};

			app.UseWebSockets(webSocketOptions);

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseSerilogRequestLogging();

			app.UseMiddleware<ExceptionMiddleware>();
			app.MapControllers();

			app.Run();
		}
	}
}
