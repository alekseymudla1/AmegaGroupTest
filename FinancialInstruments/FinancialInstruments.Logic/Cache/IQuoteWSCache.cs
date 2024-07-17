using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Cache
{
	public interface IQuoteWSCache
	{
		Task SaveQuote(string ticker, Quote quote);

		Task<Quote> GetQuote(string ticker);

		Task<Dictionary<string, Quote>> GetAllQuotes();
	}
}
