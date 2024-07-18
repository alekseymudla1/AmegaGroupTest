using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Integration.REST
{
	internal class CryptoDTO
	{
		public string Ticker { get; init; }

		public string BaseCurrency { get; init; }

		public string QuoteCurrency { get; init; }

		public IEnumerable<BookDataDTO> TopOfBookData { get; init; }

		// I prefer use methods and constructors instead of Automapper,
		// but company's codestyle is more important for me
		public Quote ToQuote()
		{
			return new Quote { Ticker = this.Ticker, Price = TopOfBookData.First().LastPrice };
		}
	}
}
