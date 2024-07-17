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

		public double BidSize { get; init; }

		public double BidPrice { get; init; }

		public double AskSize { get; init; }

		public double AskPrice { get; init; }

		public double LastSize { get; init; }

		public double LastPrice { get; init; }

		public double LastSizeNotional { get; init; }

		public string BidExchange { get; init; }

		public string AskExchange { get; init; }

		public string LastExchange { get; init; }
	}
}
