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

		public decimal BidPrice { get; init; }

		public long BidSize { get; init; }

		public decimal AskPrice { get; init; }

		public long AskSize { get; init; }

		public decimal MidPrice { get; init; }

		public Instrument ToInstrument()
		{
			return new Instrument()
			{
				Ticker = this.Ticker,
				Price = this.MidPrice
			};
		}

	}
}
