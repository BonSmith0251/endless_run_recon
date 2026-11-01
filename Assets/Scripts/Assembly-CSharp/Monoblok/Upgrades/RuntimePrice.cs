using System;
using Monoblok.Core;

namespace Monoblok.Upgrades
{
	[Serializable]
	public class RuntimePrice
	{
		private Observable<int> amount;

		public CurrencyDefinition Currency { get; }

		public Observable<int> Amount => amount;

		public RuntimePrice(Price basePrice)
		{
			Currency = basePrice.Currency;
			amount = new Observable<int>(basePrice.Amount);
		}

		public override string ToString()
		{
			return Amount.ToString();
		}
	}
}
