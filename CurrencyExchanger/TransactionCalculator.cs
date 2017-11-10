using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyExchanger
{
	public class TransactionCalculator
	{
		private readonly ICurrencyRateLoader _currencyRateLoader = new CurrencyRateLoader();
		private readonly Logger logger = new Logger();

		public TransactionCalculator(){}

		public TransactionCalculator(ICurrencyRateLoader loader)
		{
			_currencyRateLoader = loader;
		}

		public Dictionary<string, decimal> CalculateTransationsByCounterCurrency(List<Transaction> transactions)
		{
			logger.Info(Resources.TransactionsAreBeingProcessed);

			var result = transactions
				.GroupBy(t => t.CounterCurrency)
				.Select(g => new {Currency = g.Key, Sum = g.Sum(t => CalculateTransaction(t))})
				.ToDictionary(t => t.Currency, t => t.Sum);

			return result;
		}

		private decimal CalculateTransaction(Transaction transaction)
		{
			var rate = _currencyRateLoader.Load(transaction);
			return transaction.Amount * rate;
		}
	}
}
