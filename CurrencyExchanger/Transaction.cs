﻿using System;

namespace CurrencyExchanger
{
	public class Transaction
	{
		public DateTime TradeDate { get; set; }
		public string BaseCurrency { get; set; }
		public string CounterCurrency { get; set; }
		public decimal Amount { get; set; }
		public DateTime ValueDate { get; set; }

		public CurrencyRateCacheItem ToCurrenceyRateCacheItem()
		{
			return new CurrencyRateCacheItem(BaseCurrency, CounterCurrency, ValueDate);
		}
	}
}
