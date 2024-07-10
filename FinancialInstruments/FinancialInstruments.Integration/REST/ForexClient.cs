using FinancialInstruments.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FinancialInstruments.Integration.REST
{
	public class ForexClient : IForexClient, IRestClient
	{
		//https://api.tiingo.com/tiingo/crypto/top?tickers=btcusd
		private readonly string _token;

		public ForexClient(string token)
		{
			_token = token ?? throw new ArgumentNullException(nameof(token));
		}

		public async Task<Instrument> GetPrice(string ticker, CancellationToken cancellationToken = default)
		{
			using (var client = new HttpClient() { BaseAddress = Constants.TiingoForexRestUrl } )
			{
				client.DefaultRequestHeaders.Accept
					.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
				var response = await client.GetAsync($@"top?tickers={ticker}&token={_token}");
				var result = JsonConvert.DeserializeObject<IEnumerable<ForexDTO>>(await response.Content.ReadAsStringAsync());

				return result.FirstOrDefault()?.ToInstrument() ?? new Instrument() { Ticker = ticker, Price = 0 };
			}
		}
	}
}
