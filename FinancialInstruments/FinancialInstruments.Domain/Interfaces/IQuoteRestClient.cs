using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Domain.Interfaces
{
	public interface IQuoteRestClient
	{
		Task<Quote> GetQuote(string ticker, CancellationToken cancellationToken = default);
	}
}
