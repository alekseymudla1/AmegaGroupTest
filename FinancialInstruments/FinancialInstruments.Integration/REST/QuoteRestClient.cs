using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Integration.REST
{
	public class QuoteRestClient : IQuoteRestClient
	{
		private readonly IQuoteSources _quoteSources;
		private readonly IClientFactory _clientFactory;


		public QuoteRestClient(IQuoteSources quoteSources, IClientFactory clientFactory)
		{
			_quoteSources = quoteSources;
			_clientFactory = clientFactory;
		}

		public async Task<Quote> GetQuote(string ticker, CancellationToken cancellationToken = default)
		{
			var client = await GetClientByTicker(ticker);
			var result = await client.GetQuote(ticker, cancellationToken);
			return result;
		}

		private async Task<IRestClient> GetClientByTicker(string ticker)
		{
			var source = await _quoteSources.GetSource(ticker);
			var client = _clientFactory.GetClient(source);
			return client;
		}
	}
}
