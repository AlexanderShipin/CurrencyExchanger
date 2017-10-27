using System;
using System.Collections.Generic;
using CurrencyExchanger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace currencyExchangerTests
{
	[TestClass]
	public class TransactionCalculatorTests
	{
		[TestMethod]
		public void CalculateTransationsByCounterCurrencyTest()
		{
			var transactions = new List<Transaction>();
			transactions.Add(new Transaction
			{
				TradeDate = new DateTime(2014, 3, 3),
				BaseCurrency = "EUR",
				CounterCurrency = "JPY",
				Amount = 100.00m,
				ValueDate = new DateTime(2014, 6, 3)
			});
			transactions.Add(new Transaction
			{
				TradeDate = new DateTime(2014, 3, 3),
				BaseCurrency = "USD",
				CounterCurrency = "CHF",
				Amount = 200.00m,
				ValueDate = new DateTime(2014, 6, 3)
			});
			transactions.Add(new Transaction
			{
				TradeDate = new DateTime(2014, 3, 3),
				BaseCurrency = "EUR",
				CounterCurrency = "JPY",
				Amount = 10.00m,
				ValueDate = new DateTime(2014, 6, 3)
			});

			var calculatedTransactions = new TransactionCalculator(new CurrencyRateLoaderDummy()).CalculateTransationsByCounterCurrency(transactions);
			Assert.AreEqual(2, calculatedTransactions.Count);
			Assert.AreEqual(1100, calculatedTransactions["JPY"]);
			Assert.AreEqual(2000, calculatedTransactions["CHF"]);
		}
	}
}
