using System;
using Monoblok.Train;
using Monoblok.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class SlotSellView : MonoBehaviour
	{
		[SerializeField]
		private SlotView slotView;

		[SerializeField]
		private Button sellButton;

		[SerializeField]
		private RuntimePriceView priceView;

		public ModuleSlot Slot => slotView.Slot;

		public event Action<SlotView> OnSlotViewPointerEnter;

		public event Action<SlotView> OnSlotViewPointerExit;

		public event Action<SlotSellView> OnSellButtonClicked;

		private void OnEnable()
		{
			slotView.OnStateChanged += OnSlotViewStateChanged;
			slotView.OnPointerEntered += HandleSlotViewPointerEnter;
			slotView.OnPointerExited += HandleSlotViewPointerExit;
			sellButton.onClick.AddListener(HandleSellButtonClicked);
		}

		private void OnDisable()
		{
			slotView.OnStateChanged -= OnSlotViewStateChanged;
			slotView.OnPointerEntered -= HandleSlotViewPointerEnter;
			slotView.OnPointerExited -= HandleSlotViewPointerExit;
			sellButton.onClick.RemoveAllListeners();
		}

		private void HandleSlotViewPointerEnter(SlotView view)
		{
			this.OnSlotViewPointerEnter?.Invoke(view);
		}

		private void HandleSlotViewPointerExit(SlotView view)
		{
			this.OnSlotViewPointerExit?.Invoke(view);
		}

		private void HandleSellButtonClicked()
		{
			this.OnSellButtonClicked?.Invoke(this);
		}

		public void Initialize(ModuleSlot slot)
		{
			slotView.Initialize(slot);
			if (Slot != null)
			{
				Slot.OnModuleAdded -= OnModuleAdded;
			}
			if (slot != null)
			{
				slot.OnModuleAdded += OnModuleAdded;
				OnModuleAdded(slot.ModuleData);
			}
			OnSlotViewStateChanged(slotView.CurrentState);
		}

		private void OnModuleAdded(ModuleData data)
		{
			if (data == null)
			{
				priceView.gameObject.SetActive(value: false);
				return;
			}
			priceView.gameObject.SetActive(value: true);
			priceView.Initialize(data.Worth);
		}

		private void OnSlotViewStateChanged(SlotView.State newState)
		{
			switch (newState)
			{
			case SlotView.State.Inactive:
				sellButton.interactable = false;
				priceView.gameObject.SetActive(value: false);
				break;
			case SlotView.State.Free:
				sellButton.interactable = false;
				priceView.gameObject.SetActive(value: false);
				break;
			case SlotView.State.Attached:
				sellButton.interactable = true;
				priceView.gameObject.SetActive(value: true);
				break;
			}
			if (Slot != null)
			{
				_ = Slot.IsFree;
			}
		}
	}
}
