using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Cache
{
	public interface IInstrumentCache
	{
		Task SaveInstrument(Instrument instrument);

		Task<Instrument> GetInstrument(string ticker);
	}
}
