namespace FinancialInstruments.Api
{
	public class Instruments
	{
		private static readonly string[] _tickers = { "eurusd", "usdjpy", "btcusd" };

		public string[] Tickers => _tickers;
	}
}
