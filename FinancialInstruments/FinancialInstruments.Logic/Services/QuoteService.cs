using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;
using FinancialInstruments.Logic.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialInstruments.Logic.Services
{
	public class QuoteService : IQuoteService
	{
		private readonly IQuoteRestClient _quoteRestClient;
		private readonly IQuoteCache _quoteCache;
		private readonly ILogger<QuoteService> _logger;
		private readonly ClientTimeout _clientTimeout;

		public QuoteService(IQuoteRestClient quoteRestClient, 
			IQuoteCache quoteCache, 
			ILogger<QuoteService> logger,
			IOptions<ClientTimeout> options)
		{
			_quoteRestClient = quoteRestClient;
			_quoteCache = quoteCache;
			_logger = logger;
			_clientTimeout = options.Value;
		}

		public async Task<Quote> GetQuoteAsync(string ticker)
		{
			// if data receiving takes too long or exception is thrown, we take data from cache
			// anyway cache is updated after data receiving is succeded
			Task.WaitAny(Task.Delay(_clientTimeout.Timeout), UpdateCache(ticker));
			return await _quoteCache.GetQuote(ticker);
		}

		private async Task UpdateCache(string ticker)
		{
			try
			{
				var data = await _quoteRestClient.GetQuote(ticker);
				if (data != null)
				{
					await _quoteCache.SaveQuote(data);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($@"Error receiving date from client by ticker {ticker}. Exception message: {ex.Message}");
			}
		}
	}
}
