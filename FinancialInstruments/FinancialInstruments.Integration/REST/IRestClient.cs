using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Integration.REST
{
	public interface IRestClient
	{
		Task<Instrument> GetPrice(string ticker, CancellationToken cancellationToken = default);
	}
}
