using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace CurrencyExchanger
{
	public class CurrencyRateLoader
	{
		private const string DefaultSource = "fixer";
		readonly JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();
		private readonly Dictionary<string, Func<Transaction, decimal>> _loaders = new Dictionary<string, Func<Transaction, decimal>>();
		private Dictionary<CurrencyRateCacheItem, decimal> _cache = new Dictionary<CurrencyRateCacheItem, decimal>();//todo: add cache limit

		public CurrencyRateLoader()
		{
			_loaders.Add("fixer", LoadFromFixer);
		}

		public decimal Load(Transaction transaction)
		{
			return _loaders[DefaultSource](transaction);
		}

		public decimal Load(string source, Transaction transaction)
		{
			if (!_loaders.ContainsKey(source))
				source = DefaultSource;

			return _loaders[source](transaction);
		}

		public decimal LoadFromFixer(Transaction transaction)
		{
			CurrencyRateCacheItem cacheItem = transaction.ToCurrenceyRateCacheItem();
			if (_cache.ContainsKey(cacheItem))
				return _cache[cacheItem];

			var req = (HttpWebRequest) WebRequest.Create(string.Format(@"http://api.fixer.io/{0}?base={1}&symbols={2}",
				transaction.ValueDate.ToString("yyyy-MM-dd"), transaction.BaseCurrency, transaction.CounterCurrency));
			req.Method = "GET";

			Stream respStream = req.GetResponse().GetResponseStream();
			
			if (respStream == null)
				throw new Exception("No rate service response");

			var json = (new StreamReader(respStream)).ReadToEnd();

			var currencyRate = _javaScriptSerializer.Deserialize<CurrencyRateResponse>(json);

			_cache.Add(cacheItem, currencyRate.rates[transaction.CounterCurrency]);

			return currencyRate.rates[transaction.CounterCurrency];
		}
	}
}
