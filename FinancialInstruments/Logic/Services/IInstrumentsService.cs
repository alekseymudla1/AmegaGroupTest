using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Logic.Services
{
	public interface IInstrumentsService
	{
		Task<Instrument> GetInstrumentAsync(string ticker);
	}
}
