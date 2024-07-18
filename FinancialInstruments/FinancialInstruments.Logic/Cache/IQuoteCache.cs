using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Logic.Cache
{
	public interface IQuoteCache
	{
		Task SaveQuote(Quote quote);

		Task<Quote> GetQuote(string ticker);
	}
}
