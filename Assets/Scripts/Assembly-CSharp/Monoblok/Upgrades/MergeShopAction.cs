using Cysharp.Threading.Tasks;
using Monoblok.Train;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	public class MergeShopAction : ShopAction
	{
		private readonly LocalizedString notEligibleErrorMessage;

		public MergeShopAction(ShopActionContext ctx, MergeShopActionConfig config, Price price)
			: base(ctx, price, config.Hotkey, config.MetaData)
		{
			notEligibleErrorMessage = config.NotEligibleErrorMessage;
		}

		protected override void OnInitialized()
		{
			ctx.cartSelector.OnSelectionChanged += HandleSelectionChanged;
			HandleSelectionChanged(ctx.cartSelector.SelectedCart);
		}

		protected override void OnDispose()
		{
			ctx.cartSelector.OnSelectionChanged -= HandleSelectionChanged;
		}

		private void HandleSelectionChanged(TrainCart cart)
		{
			if (!(cart == null))
			{
				base.Price.Amount.SetValue(cart.CartData.Tier.MergeCost);
				Evaluate();
			}
		}

		protected override bool CanPerform(out string errorMsg)
		{
			errorMsg = string.Empty;
			if (!ctx.mergeManager.EligibleForMerge(ctx.cartSelector.SelectedCart))
			{
				errorMsg = notEligibleErrorMessage.GetLocalizedString();
				return false;
			}
			return true;
		}

		protected override async UniTask<IShopAction.ShopActionState> OnResolve()
		{
			CartMergeManager mergeManager = ctx.mergeManager;
			UniTaskCompletionSource<IShopAction.ShopActionState> tcs = new UniTaskCompletionSource<IShopAction.ShopActionState>();
			mergeManager.State.OnValueChanged += OnStateChanged;
			mergeManager.StartSelection(ctx.cartSelector.SelectedCart).Forget();
			IShopAction.ShopActionState result = await tcs.Task;
			mergeManager.State.OnValueChanged -= OnStateChanged;
			return result;
			void OnStateChanged(CartMergeManager.SelectionState s)
			{
				switch (s)
				{
				case CartMergeManager.SelectionState.Complete:
					tcs.TrySetResult(IShopAction.ShopActionState.Completed);
					break;
				case CartMergeManager.SelectionState.Canceled:
					tcs.TrySetResult(IShopAction.ShopActionState.Canceled);
					break;
				case CartMergeManager.SelectionState.Forbidden:
					tcs.TrySetResult(IShopAction.ShopActionState.Forbidden);
					break;
				}
			}
		}
	}
}
