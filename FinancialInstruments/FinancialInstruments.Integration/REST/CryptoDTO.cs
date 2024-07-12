using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Integration.REST
{
	internal class CryptoDTO
	{
		public string Ticker { get; init; }

		public string BaseCurrency { get; init; }

		public string QuoteCurrency { get; init; }

		public IEnumerable<BookDataDTO> TopOfBookData { get; init; }

		public Quote ToQuote()
		{
			return new Quote { Ticker = this.Ticker, Price = TopOfBookData.First().LastPrice };
		}
	}
}
