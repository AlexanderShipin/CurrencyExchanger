using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using Microsoft.VisualBasic.FileIO;

namespace CurrencyExchanger
{
	class Program
	{
		static void Main(string[] args)
		{
			var csvFilePath = @"..\\..\transactions-v2.csv";

			try
			{
				var transactions = GetTransactionsFromCsv(csvFilePath);
				var calculatedTransactions = CalculateTransationsByCounterCurrency(transactions);
				calculatedTransactions.ToList().ForEach(t => Console.WriteLine(String.Format("{0} {1:0.00}", t.Key, t.Value)));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.ReadLine();
		}

		private static List<Transaction> GetTransactionsFromCsv(string csvFilePath)
		{
			var result = new List<Transaction>();

			var parser = new TextFieldParser(csvFilePath) {TextFieldType = FieldType.Delimited};
			parser.SetDelimiters(",");

			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				result.Add(TransactionBuilder(fields));
			}
			return result;
		}

		private static Transaction TransactionBuilder(string[] fields)
		{
			DateTime tradeDate;
			if (!DateTime.TryParse(fields[0], out tradeDate))
				throw new Exception("Wrong date format for TradeDate " + fields[0]);

			DateTime valueDate;
			if (!DateTime.TryParse(fields[4], out valueDate))
				throw new Exception("Wrong date format for ValueDate " + fields[4]);

			return new Transaction {
				TradeDate = tradeDate,
				BaseCurrency = fields[1],
				CounterCurrency = fields[2],
				Amount = decimal.Parse(fields[3], CultureInfo.InvariantCulture),
				ValueDate = valueDate
			};
		}

		private static Dictionary<string, decimal> CalculateTransationsByCounterCurrency(List<Transaction> transactions)
		{
			var result = new Dictionary<string, decimal>();
			foreach (var t in transactions)
			{
				
				var rate = GetCurrencyRate(t.ValueDate, t.BaseCurrency, t.CounterCurrency);
				if (result.ContainsKey(t.CounterCurrency))
					result[t.CounterCurrency] += t.Amount * rate;
				else
					result[t.CounterCurrency] = t.Amount * rate;
			}
			return result;
		}

		private static decimal GetCurrencyRate(DateTime valueDate, string baseCurrency, string counterCurrency)
		{
			Thread.Sleep(150);

			var req = (HttpWebRequest)WebRequest.Create(string.Format(@"http://api.fixer.io/{0}?base={1}&symbols={2}", valueDate.ToString("yyyy-MM-dd"), baseCurrency, counterCurrency));
			req.Method = "GET";

			var respStream = req.GetResponse().GetResponseStream();
			if (respStream == null)
				throw new Exception("No rate service response");

			var json = (new StreamReader(respStream)).ReadToEnd();

			var currencyRate = new JavaScriptSerializer().Deserialize<CurrencyRateResponse>(json);
			return currencyRate.rates[counterCurrency];
		}
	}
}
