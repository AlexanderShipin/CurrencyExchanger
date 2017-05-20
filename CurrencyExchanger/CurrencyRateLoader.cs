using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;

namespace CurrencyExchanger
{
	public class CurrencyRateLoader
	{
		private const string DefaultSource = "fixer";
		readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();
		private readonly Dictionary<string, Func<DateTime, string, string, decimal>> _loaders = new Dictionary<string, Func<DateTime, string, string, decimal>>();

		public CurrencyRateLoader()
		{
			_loaders.Add("fixer", LoadFromFixer);
		}

		public decimal Load(DateTime valueDate, string baseCurrency, string counterCurrency)
		{
			return _loaders[DefaultSource](valueDate, baseCurrency, counterCurrency);
		}

		public decimal Load(string source, DateTime valueDate, string baseCurrency, string counterCurrency)
		{
			if (!_loaders.ContainsKey(source))
				source = DefaultSource;

			return _loaders[source](valueDate, baseCurrency, counterCurrency);
		}

		public decimal LoadFromFixer(DateTime valueDate, string baseCurrency, string counterCurrency)
		{
			Thread.Sleep(150);

			var req = (HttpWebRequest)WebRequest.Create(string.Format(@"http://api.fixer.io/{0}?base={1}&symbols={2}", valueDate.ToString("yyyy-MM-dd"), baseCurrency, counterCurrency));
			req.Method = "GET";

			var respStream = req.GetResponse().GetResponseStream();
			if (respStream == null)
				throw new Exception("No rate service response");

			var json = (new StreamReader(respStream)).ReadToEnd();

			var currencyRate = _javaScriptSerializer.Deserialize<CurrencyRateResponse>(json);
			return currencyRate.rates[counterCurrency];
		}
	}
}
