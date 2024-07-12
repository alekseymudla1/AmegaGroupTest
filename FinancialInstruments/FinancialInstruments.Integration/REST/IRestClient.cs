using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Integration.REST
{
	public interface IRestClient
	{
		Task<Quote> GetQuote(string ticker, CancellationToken cancellationToken = default);
	}
}
