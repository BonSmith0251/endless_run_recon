using System;
using Cysharp.Threading.Tasks;
using Monoblok.Core;
using UnityEngine.InputSystem;

namespace Monoblok.Upgrades
{
	public abstract class ShopAction : IShopAction
	{
		protected readonly ShopActionContext ctx;

		private readonly Observable<EligibilityState> eligible;

		private readonly RuntimePrice price;

		public readonly InputActionReference inputActionReference;

		public readonly ShopActionMetaData metaData;

		private bool initialized;

		public Observable<EligibilityState> Eligible => eligible;

		public RuntimePrice Price => price;

		public ShopAction(ShopActionContext ctx, Price price, InputActionReference inputActionReference = null, ShopActionMetaData metaData = null)
		{
			this.ctx = ctx;
			this.price = new RuntimePrice(price);
			this.inputActionReference = inputActionReference;
			this.metaData = metaData;
			eligible = new Observable<EligibilityState>(EligibilityState.Eligible);
		}

		public void Initialize()
		{
			ctx.wallet.Subscribe(price.Currency, HandleWalletCurrencyChanged);
			OnInitialized();
			Evaluate();
			initialized = true;
		}

		public async UniTask<IShopAction.ShopActionState> Resolve()
		{
			if (!initialized)
			{
				throw new InvalidOperationException(GetType().Name + ".Initialize() must be called before Resolve().");
			}
			Evaluate();
			if (!eligible.Value.IsEligible)
			{
				return IShopAction.ShopActionState.Forbidden;
			}
			return await OnResolve();
		}

		public void Dispose()
		{
			ctx.wallet.Unsubscribe(price.Currency, HandleWalletCurrencyChanged);
			OnDispose();
		}

		private void HandleWalletCurrencyChanged(int amount)
		{
			Evaluate();
		}

		protected void Evaluate()
		{
			bool num = ctx.wallet.CanAfford(price);
			string errorMsg;
			bool flag = CanPerform(out errorMsg);
			EligibilityState eligibilityState = ((!num) ? new EligibilityState(isEligible: false, IneligibilityReason.InsufficientFunds, ctx.insufficientFundsMessage.GetLocalizedString()) : (flag ? EligibilityState.Eligible : new EligibilityState(isEligible: false, IneligibilityReason.ConditionNotMet, errorMsg)));
			EligibilityState value = eligibilityState;
			eligible.SetValue(value);
		}

		protected abstract bool CanPerform(out string errorMsg);

		protected abstract UniTask<IShopAction.ShopActionState> OnResolve();

		protected virtual void OnInitialized()
		{
		}

		protected virtual void OnDispose()
		{
		}
	}
}
