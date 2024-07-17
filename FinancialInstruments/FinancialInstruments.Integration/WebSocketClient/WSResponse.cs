using FinancialInstruments.Domain.Models;

namespace FinancialInstruments.Integration.WebSocketClient
{
	public class WSResponse
	{
		public string Service { get; init; }

		public string MessageType { get; init; }

		public object[] Data { get; init; }

		// I prefer use methods and constructors instead of Automapper,
		// but company's codestyle is more important for me
		public Quote ToQuote()
		{
			return new Quote()
			{
				Ticker = Data[1].ToString(),
				Price = this.Service switch 
				{
					"fx" => (double)Data[4],
					"crypto_data" => (double)Data[5],
					_ => 0.0
				}
			};
		}
	}
}
