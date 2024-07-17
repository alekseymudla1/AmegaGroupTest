using FinancialInstruments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Integration.REST
{
	internal class ForexDTO
	{
		public string Ticker { get; init; }

		public DateTime QuoteTimestamp { get; init; }

		public double BidPrice { get; init; }

		public long BidSize { get; init; }

		public double AskPrice { get; init; }

		public long AskSize { get; init; }

		public double MidPrice { get; init; }

		// I prefer use methods and constructors instead of Automapper,
		// but company's codestyle is more important for me
		public Quote ToQuote()
		{
			return new Quote()
			{
				Ticker = this.Ticker,
				Price = this.MidPrice
			};
		}

	}
}
