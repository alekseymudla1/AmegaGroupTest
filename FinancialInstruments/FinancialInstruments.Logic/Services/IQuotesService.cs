using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Logic.Services
{
	public interface IQuoteService
	{
		Task<Quote> GetQuoteAsync(string ticker);
	}
}
