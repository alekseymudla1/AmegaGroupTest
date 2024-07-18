using FinancialInstruments.Domain.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FinancialInstruments.Integration.REST
{
	public class CryptoClient : ICryptoClient
	{
		private readonly string _token;

		public CryptoClient(string token)
		{
			_token = token ?? throw new ArgumentNullException(nameof(token));
		}

		public async Task<Quote> GetQuote(string ticker, CancellationToken cancellationToken = default)
		{
			using (var client = new HttpClient() { BaseAddress = Constants.TiingoCryptoRestUrl })
			{
				client.DefaultRequestHeaders.Accept
					.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
				var response = await client.GetAsync($@"top?tickers={ticker}&token={_token}");
				var result = JsonConvert.DeserializeObject<IEnumerable<CryptoDTO>>(await response.Content.ReadAsStringAsync());

				return result.FirstOrDefault().ToQuote() ?? new Quote { Ticker = ticker, Price = 0 };
			}
		}
	}
}
