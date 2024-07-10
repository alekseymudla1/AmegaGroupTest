using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Domain.Interfaces
{
	public interface IInstrumentRestClient
	{
		Task<Instrument> GetPrice(string ticker, CancellationToken cancellationToken = default);
	}
}
