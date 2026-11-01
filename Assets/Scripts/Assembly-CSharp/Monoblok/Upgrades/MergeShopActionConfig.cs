using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Shop/Actions/Merge")]
	public class MergeShopActionConfig : ShopActionConfig
	{
		[SerializeField]
		private LocalizedString notEligibleErrorMessage;

		public LocalizedString NotEligibleErrorMessage => notEligibleErrorMessage;

		public override ShopAction GetAction(ShopActionContext ctx)
		{
			return new MergeShopAction(ctx, this, ConstructPrice(0));
		}
	}
}
