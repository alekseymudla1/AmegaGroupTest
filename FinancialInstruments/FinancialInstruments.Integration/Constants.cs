using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Integration
{
	internal static class Constants
	{
		public static Uri TiingoForexRestUrl = new Uri("https://api.tiingo.com/tiingo/fx/");

		public static Uri TiingoCryptoRestUrl = new Uri("https://api.tiingo.com/tiingo/crypto/");

		public static Uri TiingoForexWSUrl = new Uri("wss://api.tiingo.com/fx");

		public static Uri TiingoCryptoWSUrl = new Uri("wss://api.tiingo.com/crypto");
	}
}
