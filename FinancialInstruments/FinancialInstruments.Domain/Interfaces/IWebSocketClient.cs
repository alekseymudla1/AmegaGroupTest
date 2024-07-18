namespace FinancialInstruments.Domain.Interfaces
{
	public interface IWebSocketClient
	{
		Task Connect();

		Task Subscribe(string ticker);
	}
}
