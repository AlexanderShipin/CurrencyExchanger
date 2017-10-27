using CurrencyExchanger;

namespace currencyExchangerTests
{
	public class CurrencyRateLoaderDummy : ICurrencyRateLoader
	{
		public decimal Load(Transaction transaction)
		{
			return 10;
		}
	}
}
