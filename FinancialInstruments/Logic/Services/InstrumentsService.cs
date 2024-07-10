using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;
using FinancialInstruments.Logic.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinancialInstruments.Logic.Services
{
	public class InstrumentsService : IInstrumentsService
	{
		private readonly IInstrumentRestClient _instrumentRestClient;
		private readonly IInstrumentCache _instrumentCache;
		private readonly ILogger<InstrumentsService> _logger;
		private readonly ClientTimeout _clientTimeout;

		public InstrumentsService(IInstrumentRestClient instrumentRestClient, 
			IInstrumentCache instrumentCache, 
			ILogger<InstrumentsService> logger,
			IOptions<ClientTimeout> options)
		{
			_instrumentRestClient = instrumentRestClient;
			_instrumentCache = instrumentCache;
			_logger = logger;
			_clientTimeout = options.Value;
		}

		public async Task<Instrument> GetInstrumentAsync(string ticker)
		{
			// if data receiving takes too long or exception is thrown, we take data from cache
			// anyway cache is updated after data receiving is succeded
			Task.WaitAny(Task.Delay(_clientTimeout.Timeout), UpdateCache(ticker));
			return await _instrumentCache.GetInstrument(ticker);
		}

		private async Task UpdateCache(string ticker)
		{
			try
			{
				var data = await _instrumentRestClient.GetPrice(ticker);
				if (data != null)
				{
					await _instrumentCache.SaveInstrument(data);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($@"Error receiving date from client by ticker {ticker}. Exception message: {ex.Message}");
			}
		}
	}
}
