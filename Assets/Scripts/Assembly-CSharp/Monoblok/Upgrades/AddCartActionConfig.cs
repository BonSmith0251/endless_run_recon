using Monoblok.SA;
using Monoblok.Train;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Shop/Actions/Add Cart")]
	public class AddCartActionConfig : ShopActionConfig
	{
		[SerializeField]
		private CartData cartData;

		[SerializeField]
		private int cartPrice = 100;

		[SerializeField]
		private int maxCartsPerStation = 2;

		[SerializeField]
		private float addCartPriceMultiplier = 1.5f;

		[SerializeField]
		private LocalizedString outOfCarsErrorMessage;

		[SerializeField]
		private LocalizedString trainFullErrorMessage;

		[SerializeField]
		private GameEvent resolveEvent;

		public CartData CartData => cartData;

		public int CartPrice => cartPrice;

		public int MaxCartsPerStation => maxCartsPerStation;

		public float AddCartPriceMultiplier => addCartPriceMultiplier;

		public LocalizedString OutOfCarsErrorMessage => outOfCarsErrorMessage;

		public LocalizedString TrainFullErrorMessage => trainFullErrorMessage;

		public GameEvent ResolveEvent => resolveEvent;

		public override ShopAction GetAction(ShopActionContext ctx)
		{
			return new AddCartAction(ctx, this, ConstructPrice(cartPrice));
		}
	}
}
