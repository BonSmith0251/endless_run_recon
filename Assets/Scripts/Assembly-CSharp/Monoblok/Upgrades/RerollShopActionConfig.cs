using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Shop/Actions/Reroll")]
	public class RerollShopActionConfig : ShopActionConfig
	{
		[SerializeField]
		[Min(0f)]
		private float rerollPricePercent = 0.2f;

		[SerializeField]
		private float rerollPriceMultiplier = 2f;

		[SerializeField]
		private GameEvent performEvent;

		public float RerollPricePercent => rerollPricePercent;

		public float RerollPriceMultiplier => rerollPriceMultiplier;

		public GameEvent PerformEvent => performEvent;

		public override ShopAction GetAction(ShopActionContext ctx)
		{
			return new RerollShopAction(ctx, this, ConstructPrice(0));
		}
	}
}
