using FinancialInstruments.Domain.Models;
using System.Collections.Concurrent;

namespace FinancialInstruments.Logic.Cache
{
	public class QuoteWSCache : IQuoteWSCache
	{
		private readonly ConcurrentDictionary<string, Quote> _quoteDictionary = new ConcurrentDictionary<string, Quote>();

		public QuoteWSCache()
		{

		}

		public async Task<Dictionary<string, Quote>> GetAllQuotes()
		{
			return _quoteDictionary.ToDictionary();
		}

		public async Task<Quote> GetQuote(string ticker)
		{
			return _quoteDictionary.GetValueOrDefault(ticker);
		}

		public async Task SaveQuote(string ticker, Quote quote)
		{
			_quoteDictionary.AddOrUpdate(ticker, _ => quote, (key, oldQuote) => quote);
		}
	}
}
