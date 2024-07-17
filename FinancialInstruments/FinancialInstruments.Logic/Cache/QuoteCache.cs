using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Cache
{
	public class QuoteCache : IQuoteCache
	{
		private readonly ConcurrentDictionary<string, Quote> _quotes = new ConcurrentDictionary<string, Quote>();
		public QuoteCache() { }

		public async Task<Quote> GetQuote(string ticker)
		{
			return _quotes.TryGetValue(ticker, out Quote quote) ? quote : null;
		}

		public async Task SaveQuote(Quote quote)
		{
			_quotes.AddOrUpdate(quote.Ticker, _ => quote, (ticker, oldQuote) => quote);
		}
	}
}
