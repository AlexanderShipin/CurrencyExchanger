using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchanger
{
	public interface ICurrencyRateLoader
	{
		decimal Load(Transaction transaction);
	}
}
