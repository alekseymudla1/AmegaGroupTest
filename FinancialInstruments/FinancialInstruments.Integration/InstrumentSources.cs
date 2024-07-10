namespace FinancialInstruments.Integration
{
	// This class emulates source of instruments sources data
	// It would be database, cache or any other storage
	public class InstrumentSources : IInstrumentSources
	{
		private readonly Dictionary<string, Source> _instrumentSourcesDict = new Dictionary<string, Source>()
		{
			{ "eurusd", Source.Forex },
			{ "usdjpy", Source.Forex },
			{ "btcusd", Source.Crypto }
		};
		public InstrumentSources() { }

		public async Task<Source> GetSource(string ticker)
		{
			if (_instrumentSourcesDict.ContainsKey(ticker))
			{
				return _instrumentSourcesDict[ticker];
			}
			else
			{
				throw new Exception($@"Ticker {ticker} does not exists");
			}
		}
	}
}
