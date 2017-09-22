using System;
using System.Collections.Generic;

namespace CurrencyExchanger
{
	public class TransactionCalculator
	{
		private readonly CurrencyRateLoader _currencyRateLoader = new CurrencyRateLoader();
		private readonly Logger logger = new Logger();

		public Dictionary<string, decimal> CalculateTransationsByCounterCurrency(List<Transaction> transactions)
		{
			var result = new Dictionary<string, decimal>();
			var stringCounter = 0;
			foreach (var t in transactions)
			{
				var value = CalculateTransaction(t);
				if (result.ContainsKey(t.CounterCurrency))
					result[t.CounterCurrency] += value;
				else
					result[t.CounterCurrency] = value;

				stringCounter++;
				if (stringCounter % 10 == 0)
					logger.Info(String.Format(Resources.ProcessedNTransactions, stringCounter));
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
