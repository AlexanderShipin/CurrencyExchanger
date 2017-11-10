using System;

namespace CurrencyExchanger
{
	public class Logger
	{
		public void Error(Exception e)
		{
			Console.WriteLine(Resources.Error, e.Message);
		}

		public void Info(String m)
		{
			Console.WriteLine(m);
		}
	}
}
