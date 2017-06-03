using System.Collections.Generic;

namespace CurrencyExchanger
{
	public class TransactionCalculator
	{
		readonly CurrencyRateLoader _currencyRateLoader = new CurrencyRateLoader();

		public Dictionary<string, decimal> CalculateTransationsByCounterCurrency(List<Transaction> transactions)
		{
			var result = new Dictionary<string, decimal>();
			foreach (var t in transactions)
			{
				var value = CalculateTransaction(t);
				if (result.ContainsKey(t.CounterCurrency))
					result[t.CounterCurrency] += value;
				else
					result[t.CounterCurrency] = value;
			}
			return result;
		}

		private decimal CalculateTransaction(Transaction transaction)
		{
			var rate = _currencyRateLoader.Load(transaction);
			return transaction.Amount * rate;
		}
	}
}
