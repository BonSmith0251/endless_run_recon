using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Monoblok.Upgrades
{
	public class RerollShopAction : ShopAction
	{
		private readonly RerollShopActionConfig config;

		private readonly float priceMultiplier;

		private readonly float startingPercent;

		public RerollShopAction(ShopActionContext ctx, RerollShopActionConfig config, Price price)
			: base(ctx, price, config.Hotkey, config.MetaData)
		{
			this.config = config;
			priceMultiplier = config.RerollPriceMultiplier;
			startingPercent = config.RerollPricePercent;
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			int value = Mathf.RoundToInt((float)ctx.wallet.GetCount(base.Price.Currency) * startingPercent);
			base.Price.Amount.SetValue(value);
		}

		protected override bool CanPerform(out string errorMsg)
		{
			errorMsg = string.Empty;
			return true;
		}

		protected override UniTask<IShopAction.ShopActionState> OnResolve()
		{
			ctx.shop.RerollShop();
			base.Price.Amount.SetValue(Mathf.RoundToInt((float)base.Price.Amount.Value * priceMultiplier));
			if (config.PerformEvent != null)
			{
				config.PerformEvent.TriggerGameEvent();
			}
			return UniTask.FromResult(IShopAction.ShopActionState.Completed);
		}
	}
}
