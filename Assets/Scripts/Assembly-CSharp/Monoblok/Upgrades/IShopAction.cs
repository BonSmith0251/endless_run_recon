using Cysharp.Threading.Tasks;
using Monoblok.Core;

namespace Monoblok.Upgrades
{
	public interface IShopAction
	{
		public enum ShopActionState
		{
			Completed,
			Canceled,
			Forbidden
		}

		Observable<EligibilityState> Eligible { get; }

		RuntimePrice Price { get; }

		UniTask<ShopActionState> Resolve();

		void Initialize();

		void Dispose();
	}
}
