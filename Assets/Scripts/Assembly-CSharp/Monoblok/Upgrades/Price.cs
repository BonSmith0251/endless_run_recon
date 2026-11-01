using System;

namespace Monoblok.Upgrades
{
	[Serializable]
	public struct Price
	{
		public CurrencyDefinition Currency;

		public int Amount;
	}
}
