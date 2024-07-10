using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Integration
{
	public interface IInstrumentSources
	{
		Task<Source> GetSource(string ticker);
	}
}
