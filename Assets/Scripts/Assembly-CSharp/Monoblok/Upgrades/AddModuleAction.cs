using Cysharp.Threading.Tasks;
using Monoblok.Core;
using Monoblok.Train;
using UnityEngine;
using UnityEngine.Localization;
using ZLinq;

namespace Monoblok.Upgrades
{
	public class AddModuleAction : ShopAction
	{
		private readonly ModuleDefinition moduleDefinition;

		private readonly Observable<bool> isResolved = new Observable<bool>(value: false);

		private readonly Price originalPrice;

		private readonly float modulePriceMultiplier;

		private readonly LocalizedString noFreeSlotsMessage;

		private bool isLocked;

		public bool IsLocked => isLocked;

		public Observable<bool> IsResolved => isResolved;

		public ModuleDefinition ModuleDefinition => moduleDefinition;

		public AddModuleAction(ModuleDefinition moduleDefinition, ShopActionContext ctx, Price price, LocalizedString noFreeSlotsMessage, float modulePriceMultiplier = 0.5f)
			: base(ctx, price)
		{
			originalPrice = price;
			this.moduleDefinition = moduleDefinition;
			this.noFreeSlotsMessage = noFreeSlotsMessage;
			this.modulePriceMultiplier = modulePriceMultiplier;
		}

		public void ToggleLock()
		{
			isLocked = !isLocked;
		}

		protected override void OnInitialized()
		{
			ctx.train.OnCartsChanged += base.Evaluate;
		}

		protected override void OnDispose()
		{
			ctx.train.OnCartsChanged -= base.Evaluate;
		}

		protected override bool CanPerform(out string errorMsg)
		{
			errorMsg = string.Empty;
			TrainCart selectedCart = ctx.cartSelector.SelectedCart;
			if (selectedCart == null)
			{
				return false;
			}
			if (!selectedCart.ModuleManager.Slots.AsValueEnumerable().Any((ModuleSlot s) => s.IsFree))
			{
				errorMsg = noFreeSlotsMessage.GetLocalizedString();
				return false;
			}
			return moduleDefinition.CanBePlaced(selectedCart, out errorMsg);
		}

		protected override UniTask<IShopAction.ShopActionState> OnResolve()
		{
			TrainCart selectedCart = ctx.cartSelector.SelectedCart;
			if (selectedCart == null)
			{
				return UniTask.FromResult(IShopAction.ShopActionState.Forbidden);
			}
			ModuleManager moduleManager = selectedCart.ModuleManager;
			ModuleData runtimeData = moduleDefinition.GetRuntimeData();
			int value = Mathf.RoundToInt((float)runtimeData.Worth.Amount.Value * modulePriceMultiplier);
			runtimeData.Worth.Amount.SetValue(value);
			if (moduleManager.TryAddModule(runtimeData))
			{
				isResolved.SetValue(newValue: true);
				isLocked = false;
				return UniTask.FromResult(IShopAction.ShopActionState.Completed);
			}
			return UniTask.FromResult(IShopAction.ShopActionState.Forbidden);
		}
	}
}
