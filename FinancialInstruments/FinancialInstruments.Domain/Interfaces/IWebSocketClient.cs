using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Domain.Interfaces
{
	public interface IWebSocketClient
	{
		Task Connect();

		Task Subscribe(string ticker);
	}
}
