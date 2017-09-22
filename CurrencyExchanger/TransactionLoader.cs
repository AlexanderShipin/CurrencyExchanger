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
		private readonly Logger logger = new Logger();

		public TransactionLoader()
		{
			_loaders.Add(".csv", GetTransactionsFromCsv);
		}

		public List<Transaction> Load(string csvFilePath)
		{
			var extension = Path.GetExtension(csvFilePath);

			if (String.IsNullOrEmpty(extension) || !_loaders.ContainsKey(extension))
				throw new Exception(Resources.FileFormatIsNotSupported);

			return _loaders[extension](csvFilePath);
		}

		private List<Transaction> GetTransactionsFromCsv(string csvFilePath)
		{
			var result = new List<Transaction>();

			var parser = new TextFieldParser(csvFilePath) { TextFieldType = FieldType.Delimited };
			parser.SetDelimiters(Settings.CsvDelimiter);

			var lineCounter = 0;
			var brokenLinesCounter = 0;
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();
				try
				{
					result.Add(TransactionBuilder(fields));
				}
				catch (Exception e)
				{
					logger.Error(e);
					brokenLinesCounter++;
				}

				lineCounter++;
				if (lineCounter % 10 == 0)
					logger.Info(String.Format(Resources.LoadedNTransactions, lineCounter));
			}
			logger.Info(String.Format(Resources.NTransactionsWereNotLoaded, brokenLinesCounter));
			return result;
		}

		private Transaction TransactionBuilder(string[] fields)
		{
			DateTime tradeDate;
			if (!DateTime.TryParseExact(fields[0], Settings.DatePattern, Settings.DateNumberFormatCulture, DateTimeStyles.None, out tradeDate))
				throw new Exception(String.Format(Resources.WrongDateFormatForTradeDate, fields[0]));

			decimal amount;
			if (!Decimal.TryParse(fields[3], NumberStyles.Number, Settings.DateNumberFormatCulture, out amount))
				throw new Exception(String.Format(Resources.WrongDecimalFormatForAmount, fields[3]));

			DateTime valueDate;
			if (!DateTime.TryParseExact(fields[4], Settings.DatePattern, Settings.DateNumberFormatCulture, DateTimeStyles.None, out valueDate))
				throw new Exception(String.Format(Resources.WrongDecimalFormatForAmount, fields[4]));

			if (fields[1] == String.Empty)
				throw new Exception(Resources.BaseCurrencyIsEmpty);

			if (fields[2] == String.Empty)
				throw new Exception(Resources.CounterCurrencyIsEmpty);

			return new Transaction
			{
				TradeDate = tradeDate,
				BaseCurrency = fields[1],
				CounterCurrency = fields[2],
				Amount = amount,
				ValueDate = valueDate
			};
		}
	}
}
