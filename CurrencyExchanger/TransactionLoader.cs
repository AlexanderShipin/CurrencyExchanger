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
				throw new Exception("File format is not supported");

			return _loaders[extension](csvFilePath);
		}

		private List<Transaction> GetTransactionsFromCsv(string csvFilePath)
		{
			var result = new List<Transaction>();

			var parser = new TextFieldParser(csvFilePath) { TextFieldType = FieldType.Delimited };
			parser.SetDelimiters(",");

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
                    logger.Info("Loaded " + lineCounter + " transactions");
			}
            logger.Info(brokenLinesCounter + " transactions were not loaded");
			return result;
		}

		private Transaction TransactionBuilder(string[] fields)
		{
			DateTime tradeDate;
			if (!DateTime.TryParse(fields[0], out tradeDate))
				throw new Exception("Wrong date format for TradeDate " + fields[0]);

            decimal amount = 0;
            if (!Decimal.TryParse(fields[3], NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
                throw new Exception("Wrong decimal format for Amount " + fields[3]);

            DateTime valueDate;
			if (!DateTime.TryParse(fields[4], out valueDate))
				throw new Exception("Wrong date format for ValueDate " + fields[4]);

            if (fields[1] == "")
                throw new Exception("BaseCurrency is empty");

            if (fields[2] == "")
                throw new Exception("CounterCurrency is empty");

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
