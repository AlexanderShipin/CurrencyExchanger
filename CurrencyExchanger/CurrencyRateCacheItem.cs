using System;

namespace CurrencyExchanger
{
	public class CurrencyRateCacheItem
	{
		public string BaseCurrency { get; set; }
		public string CounterCurrency { get; set; }
		public DateTime ValueDate { get; set; }

		public CurrencyRateCacheItem()
		{
		}

		public CurrencyRateCacheItem(string baseCurrency, string counterCurrency, DateTime valueDate) : this()
		{
			BaseCurrency = baseCurrency;
			CounterCurrency = counterCurrency;
			ValueDate = valueDate;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((CurrencyRateCacheItem) obj);
		}
		protected bool Equals(CurrencyRateCacheItem other)
		{
			return string.Equals(BaseCurrency, other.BaseCurrency) && string.Equals(CounterCurrency, other.CounterCurrency) && ValueDate.Equals(other.ValueDate);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (BaseCurrency != null ? BaseCurrency.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (CounterCurrency != null ? CounterCurrency.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
				return hashCode;
			}
		}
	}
}
