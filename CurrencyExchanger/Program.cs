using System;
using System.Linq;

namespace CurrencyExchanger
{
	class Program
	{
		static void Main(string[] args)
		{
			Logger logger = new Logger();

			try
			{
				var transactions = new TransactionLoader().Load(Settings.CsvFilePath);
				var calculatedTransactions = new TransactionCalculator().CalculateTransationsByCounterCurrency(transactions);

				Console.WriteLine(String.Empty);
				Console.WriteLine(Resources.Summary);
				calculatedTransactions.ToList().ForEach(t => Console.WriteLine($"{t.Key} {t.Value:0.00}"));
			}
			catch (Exception e)
			{
				logger.Error(e);
			}

			Console.ReadLine();
		}
	}
}
