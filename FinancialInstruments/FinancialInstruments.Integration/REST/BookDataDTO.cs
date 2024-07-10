using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Integration.REST
{
	internal class BookDataDTO
	{
		public DateTime QuoteTimestamp { get; init; }

		public DateTime LastSaleTimestamp { get; init; }

		public decimal BidSize { get; init; }

		public decimal BidPrice { get; init; }

		public decimal AskSize { get; init; }

		public decimal AskPrice { get; init; }

		public decimal LastSize { get; init; }

		public decimal LastPrice { get; init; }

		public decimal LastSizeNotional { get; init; }

		public string BidExchange { get; init; }

		public string AskExchange { get; init; }

		public string LastExchange { get; init; }
	}
}
