using FMODUnity;
using Monoblok.Inventory;
using Monoblok.SA;
using Monoblok.Train;
using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok
{
	public class CartInspector : MonoBehaviour
	{
		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private CartInspectorView inspectorView;

		[SerializeField]
		private CartSelector cartSelector;

		[SerializeField]
		private CartMergeManager cartMergeManager;

		[SerializeField]
		private CartTradeManager cartTradeManager;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private ErrorView errorView;

		[SerializeField]
		private EventReference sellRef;

		[SerializeField]
		private GameEvent sellEvent;

		private TrainCart currentCart;

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				inspectorView.OnSellModuleClicked += HandleSellModuleClicked;
				inspectorView.OnTradeClicked += HandleTradeClicked;
				cartSelector.OnSelectionChanged += OnCartSelected;
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				inspectorView.OnSellModuleClicked -= HandleSellModuleClicked;
				inspectorView.OnTradeClicked -= HandleTradeClicked;
				cartSelector.OnSelectionChanged -= OnCartSelected;
			}
		}

		private void OnCartSelected(TrainCart cart)
		{
			if (!(cart == null))
			{
				currentCart = cart;
				inspectorView.Initialize(cart, cartTradeManager.CanTradeCart(cart));
			}
		}

		private void OnMergeStateChanged(CartMergeManager.SelectionState state)
		{
			if (state != CartMergeManager.SelectionState.Selecting)
			{
				cartMergeManager.State.OnValueChanged -= OnMergeStateChanged;
			}
		}

		private void HandleSellModuleClicked(SlotSellView view)
		{
			ModuleSlot slot = view.Slot;
			if (slot == null || slot.IsFree)
			{
				return;
			}
			ModuleData moduleData = slot.ModuleData;
			if (!moduleData.Definition.CanBeSold(currentCart, out var errorMsg))
			{
				errorView.Show(errorMsg);
				return;
			}
			slot.RemoveModule();
			wallet.AddCurrency(moduleData.Worth.Currency, moduleData.Worth.Amount.Value);
			RuntimeManager.PlayOneShot(sellRef);
			if (sellEvent != null)
			{
				sellEvent.TriggerGameEvent();
			}
		}

		private void HandleTradeClicked()
		{
			cartTradeManager.InitializeTrade(currentCart);
		}
	}
}
