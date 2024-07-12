using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Cache
{
	public class QuoteCache : IQuoteCache
	{
		private readonly Dictionary<string, Quote> _quotes = new Dictionary<string, Quote>();
		public QuoteCache() { }

		public async Task<Quote> GetQuote(string ticker)
		{
			return _quotes.TryGetValue(ticker, out Quote quote) ? quote : null;
		}

		public async Task SaveQuote(Quote quote)
		{
			if(_quotes.ContainsKey(quote.Ticker))
			{
				_quotes[quote.Ticker] = quote;
			}
			else
			{
				_quotes.Add(quote.Ticker, quote);
			}
		}
	}
}
