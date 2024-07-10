using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Cache
{
	public class InstrumentCache : IInstrumentCache
	{
		private readonly Dictionary<string, Instrument> _instruments = new Dictionary<string, Instrument>();
		public InstrumentCache() { }

		public async Task<Instrument> GetInstrument(string ticker)
		{
			return _instruments.TryGetValue(ticker, out Instrument instrument) ? instrument : null;
		}

		public async Task SaveInstrument(Instrument instrument)
		{
			if(_instruments.ContainsKey(instrument.Ticker))
			{
				_instruments[instrument.Ticker] = instrument;
			}
			else
			{
				_instruments.Add(instrument.Ticker, instrument);
			}
		}
	}
}
