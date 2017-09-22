using System.Globalization;

namespace CurrencyExchanger
{
	public static class Settings
	{
		public const string CsvFilePath = @"..\\..\transactions-v2.csv";
		public const string CsvDelimiter = ",";
		public const string DatePattern = "yyyy-mm-dd";
		public static readonly CultureInfo DateNumberFormatCulture = CultureInfo.InvariantCulture;
	}
}
