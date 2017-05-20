using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace CurrencyExchanger
{
	public class TransactionLoader
	{
		private readonly Dictionary<string, Func<string, List<Transaction>>> _loaders = new Dictionary<string, Func<string, List<Transaction>>>();

		public TransactionLoader()
		{
			_loaders.Add("csv", GetTransactionsFromCsv);
		}

		public List<Transaction> Load(string csvFilePath)
		{
			var extension = Path.GetExtension(csvFilePath);

			if (String.IsNullOrEmpty(extension) || !_loaders.ContainsKey(extension))
				throw new Exception("File format is not supported");

			return _loaders[extension](csvFilePath);
		}

		private List<Transaction> GetTransactionsFromCsv(string csvFilePath)
		{
			var result = new List<Transaction>();

			var parser = new TextFieldParser(csvFilePath) { TextFieldType = FieldType.Delimited };
			parser.SetDelimiters(",");

			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				result.Add(TransactionBuilder(fields));
			}
			return result;
		}

		private Transaction TransactionBuilder(string[] fields)
		{
			DateTime tradeDate;
			if (!DateTime.TryParse(fields[0], out tradeDate))
				throw new Exception("Wrong date format for TradeDate " + fields[0]);

			DateTime valueDate;
			if (!DateTime.TryParse(fields[4], out valueDate))
				throw new Exception("Wrong date format for ValueDate " + fields[4]);

			return new Transaction
				{
					TradeDate = tradeDate,
					BaseCurrency = fields[1],
					CounterCurrency = fields[2],
					Amount = decimal.Parse(fields[3], CultureInfo.InvariantCulture),
					ValueDate = valueDate
				};
		}
	}
}
