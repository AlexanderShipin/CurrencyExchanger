using System;
using System.Linq;

namespace CurrencyExchanger
{
	class Program
	{
		static void Main(string[] args)
		{
			var csvFilePath = @"..\\..\transactions-v2.csv";

			try
			{
				var transactions = new TransactionLoader().Load(csvFilePath);
				var calculatedTransactions = new TransactionCalculator().CalculateTransationsByCounterCurrency(transactions);

				calculatedTransactions.ToList().ForEach(t => Console.WriteLine(String.Format("{0} {1:0.00}", t.Key, t.Value)));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadLine();
		}
	}
}
