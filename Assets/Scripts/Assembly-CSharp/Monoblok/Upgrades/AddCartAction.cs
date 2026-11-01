using Cysharp.Threading.Tasks;
using Monoblok.Core;
using Monoblok.Train;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	public class AddCartAction : ShopAction
	{
		private readonly AddCartActionConfig config;

		public readonly int MaxPerformCount;

		private readonly CartData cartData;

		private readonly Price originalPrice;

		private readonly float priceMultiplier;

		private readonly LocalizedString outOfCarsErrorMessage;

		private readonly LocalizedString trainFullErrorMessage;

		private Observable<int> performCount;

		public Observable<int> PerformCount => performCount;

		public AddCartAction(ShopActionContext ctx, AddCartActionConfig config, Price price)
			: base(ctx, price, config.Hotkey, config.MetaData)
		{
			this.config = config;
			cartData = config.CartData;
			originalPrice = price;
			priceMultiplier = config.AddCartPriceMultiplier;
			MaxPerformCount = config.MaxCartsPerStation;
			outOfCarsErrorMessage = config.OutOfCarsErrorMessage;
			trainFullErrorMessage = config.TrainFullErrorMessage;
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			performCount = new Observable<int>(0);
			base.Price.Amount.SetValue(originalPrice.Amount);
		}

		protected override bool CanPerform(out string errorMsg)
		{
			errorMsg = string.Empty;
			if (MaxPerformCount > 0 && performCount.Value >= MaxPerformCount)
			{
				errorMsg = outOfCarsErrorMessage.GetLocalizedString();
				return false;
			}
			if (ctx.train.IsFull)
			{
				errorMsg = trainFullErrorMessage.GetLocalizedString();
				return false;
			}
			return true;
		}

		protected override UniTask<IShopAction.ShopActionState> OnResolve()
		{
			if (!ctx.train.TryAddCart(cartData, out var cart))
			{
				return UniTask.FromResult(IShopAction.ShopActionState.Forbidden);
			}
			performCount.SetValue(performCount.Value + 1);
			base.Price.Amount.SetValue(Mathf.RoundToInt((float)base.Price.Amount.Value * priceMultiplier));
			ctx.cartSelector.ForceSelection(cart);
			if (config.ResolveEvent != null)
			{
				config.ResolveEvent.TriggerGameEvent();
			}
			return UniTask.FromResult(IShopAction.ShopActionState.Completed);
		}
	}
}
