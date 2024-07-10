using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Integration.REST
{
	public class InstrumentRestClient : IInstrumentRestClient
	{
		private readonly IInstrumentSources _instrumentSources;
		private readonly IClientFactory _clientFactory;


		public InstrumentRestClient(IInstrumentSources instrumentSources, IClientFactory clientFactory) 
		{
			_instrumentSources = instrumentSources;
			_clientFactory = clientFactory;
		}

		public async Task<Instrument> GetPrice(string ticker, CancellationToken cancellationToken = default)
		{
			var client = await GetClientByTicker(ticker);
			var result = await client.GetPrice(ticker, cancellationToken);
			return result;
		}

		private async Task<IRestClient> GetClientByTicker(string ticker)
		{
			var source = await _instrumentSources.GetSource(ticker);
			var client = await _clientFactory.GetClient(source);
			return client;
		}
	}
}
