using Monoblok.SA;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Shop/Actions/Repair")]
	public class RepairCartsActionConfig : ShopActionConfig
	{
		[SerializeField]
		private int coinPerHp = 1;

		[SerializeField]
		private LocalizedString maxHealthErrorMessage;

		[SerializeField]
		private GameEvent performeEvent;

		public GameEvent PerformEvent => performeEvent;

		public int CoinPerHp => coinPerHp;

		public LocalizedString MaxHealthErrorMessage => maxHealthErrorMessage;

		public override ShopAction GetAction(ShopActionContext ctx)
		{
			return new RepairCartsAction(ctx, this, ConstructPrice(0));
		}
	}
}
