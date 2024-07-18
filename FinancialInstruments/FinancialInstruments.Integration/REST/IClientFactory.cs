namespace FinancialInstruments.Integration.REST
{
	public interface IClientFactory
	{
		IRestClient GetClient(Source source);
	}
}
