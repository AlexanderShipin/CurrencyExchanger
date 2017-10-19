using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CurrencyExchanger;

namespace currencyExchangerTests
{
	[TestClass]
	public class TransactionLoaderTests
	{
		[TestMethod]
		public void LoaderLoadsTransactionsFromCsvFile()
		{
			var csvFilePath = @"..\\..\transactions-test.csv";
			var loader = new TransactionLoader();
			var transactions = loader.Load(csvFilePath);
			Assert.AreEqual(50, transactions.Count);
		}

		[TestMethod]
		public void LoaderFileNotFound()
		{
			var csvFilePath = @"..\\..\transactions-test1.csv";
			var loader = new TransactionLoader();
			try
			{
				var transactions = loader.Load(csvFilePath);
			}
			catch (Exception e)
			{
				Assert.IsTrue(e.Message.Contains("Could not find file"));
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void LoaderDoesNotLoadUnknowExtensionFile()
		{
			var csvFilePath = @"..\\..\transactions-test.csv1";
			var loader = new TransactionLoader();
			try
			{
				var transactions = loader.Load(csvFilePath);
			}
			catch (Exception e)
			{
				Assert.AreEqual(Resources.FileFormatIsNotSupported, e.Message);
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void LoaderDoesNotLoadTransactionWithWrongData()
		{
			var csvFilePath = @"..\\..\transactions-wrong-data-test.csv";
			var loader = new TransactionLoader();
			var transactions = loader.Load(csvFilePath);
			Assert.AreEqual(0, transactions.Count);
		}
	}
}
