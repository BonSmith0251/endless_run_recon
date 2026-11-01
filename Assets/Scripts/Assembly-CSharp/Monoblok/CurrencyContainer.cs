using Monoblok.Core;

namespace Monoblok
{
	public class CurrencyContainer
	{
		public readonly Currency Currency;

		private Observable<int> count;

		public Observable<int> Count => count;

		public CurrencyContainer(Currency currency)
		{
			Currency = currency;
			count = new Observable<int>(0);
		}
	}
}
