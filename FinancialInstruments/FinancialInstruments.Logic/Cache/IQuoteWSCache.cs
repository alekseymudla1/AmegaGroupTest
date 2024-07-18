using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Logic.Cache
{
	public interface IQuoteWSCache
	{
		Task SaveQuote(string ticker, Quote quote);

		Task<Quote> GetQuote(string ticker);

		Task<Dictionary<string, Quote>> GetAllQuotes();
	}
}
