using Cysharp.Threading.Tasks;
using Monoblok.DamageSystem;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	public class RepairCartsAction : ShopAction
	{
		private readonly RepairCartsActionConfig config;

		private readonly int coinPerHp;

		private readonly LocalizedString maxHealthErrorMessage;

		public RepairCartsAction(ShopActionContext ctx, RepairCartsActionConfig config, Price price)
			: base(ctx, price, config.Hotkey, config.MetaData)
		{
			this.config = config;
			coinPerHp = config.CoinPerHp;
			maxHealthErrorMessage = config.MaxHealthErrorMessage;
		}

		protected override void OnInitialized()
		{
			CalculatePrice();
		}

		private void CalculatePrice()
		{
			Health health = ctx.train.Health;
			int num = health.MaxHealth - health.CurrentHealth;
			base.Price.Amount.SetValue(num * coinPerHp);
		}

		protected override bool CanPerform(out string errorMsg)
		{
			errorMsg = string.Empty;
			Health health = ctx.train.Health;
			if (health.CurrentHealth >= health.MaxHealth)
			{
				errorMsg = maxHealthErrorMessage.GetLocalizedString();
				return false;
			}
			return true;
		}

		protected override UniTask<IShopAction.ShopActionState> OnResolve()
		{
			if (config != null && config.PerformEvent != null)
			{
				config.PerformEvent.TriggerGameEvent();
			}
			ctx.train.Health.Heal(ctx.train.Health.MaxHealth);
			CalculatePrice();
			return UniTask.FromResult(IShopAction.ShopActionState.Completed);
		}
	}
}
