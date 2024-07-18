namespace FinancialInstruments.Integration
{
	public interface IQuoteSources
	{
		Task<Source> GetSource(string ticker);
	}
}
