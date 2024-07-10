using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Domain.Models
{
	public class Instrument
	{
		public string Ticker { get; set; }

		public decimal Price { get; set; }
	}
}
