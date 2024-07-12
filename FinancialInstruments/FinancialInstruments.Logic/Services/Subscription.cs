using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Logic.Services
{
	public record Subscription(WebSocket WebSocket, string Ticker);
}
