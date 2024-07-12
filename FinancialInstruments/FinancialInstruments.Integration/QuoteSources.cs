namespace FinancialInstruments.Integration
{
	// This class emulates source of instruments sources data
	// It would be database, cache or any other storage
	public class QuoteSources : IQuoteSources
	{
		private readonly Dictionary<string, Source> _quoteSourcesDict = new Dictionary<string, Source>()
		{
			{ "eurusd", Source.Forex },
			{ "usdjpy", Source.Forex },
			{ "btcusd", Source.Crypto }
		};
		public QuoteSources() { }

		public async Task<Source> GetSource(string ticker)
		{
			if (_quoteSourcesDict.ContainsKey(ticker))
			{
				return _quoteSourcesDict[ticker];
			}
			else
			{
				throw new Exception($@"Ticker {ticker} does not exists");
			}
		}
	}
}
