using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Domain.Models
{
	public class ClientTimeoutOptions
	{
		public const string Section = "ClientTimeout";
		public int Timeout { get; set; }
	}
}
