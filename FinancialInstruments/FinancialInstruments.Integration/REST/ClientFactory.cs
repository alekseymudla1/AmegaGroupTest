namespace FinancialInstruments.Integration.REST
{
	public class ClientFactory : IClientFactory
	{
		private readonly IForexClient _forexClient;
		private readonly ICryptoClient _cryptoClient;

		public ClientFactory(IForexClient forexClient, ICryptoClient cryptoClient)
		{
			_forexClient = forexClient;
			_cryptoClient = cryptoClient;
		}

		public IRestClient GetClient(Source source)
		{
			return source switch
			{
				Source.Forex => _forexClient,
				Source.Crypto => _cryptoClient,
				_ => throw new ArgumentOutOfRangeException(nameof(source), $@"Not expected source: {source}")
			};
		}
	}
}
