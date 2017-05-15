using System;
using System.Collections.Generic;

namespace CurrencyExchanger
{
	public class CurrencyRateResponse
	{
		public string @base { get; set; }
		public DateTime date { get; set; }
		public Dictionary<string, decimal> rates { get; set; }
	}
}
