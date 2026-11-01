using System;
using System.Collections.Generic;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	public class SlotCollectionView : MonoBehaviour
	{
		[SerializeField]
		private SlotTooltip slotTooltip;

		private SlotSellView[] slotViews;

		public event Action<SlotSellView> OnSellModuleClicked;

		private void Awake()
		{
			slotViews = GetComponentsInChildren<SlotSellView>();
		}

		public void Initialize(IEnumerable<ModuleSlot> slots)
		{
			Clear();
			Queue<ModuleSlot> queue = new Queue<ModuleSlot>(slots);
			SlotSellView[] array = slotViews;
			foreach (SlotSellView slotSellView in array)
			{
				if (queue.Count == 0)
				{
					slotSellView.Initialize(null);
					continue;
				}
				slotSellView.OnSlotViewPointerEnter += HandleSlotPointerEnter;
				slotSellView.OnSlotViewPointerExit += HandleSlotPointerExit;
				slotSellView.OnSellButtonClicked += HandleSellButtonClicked;
				slotSellView.Initialize(queue.Dequeue());
			}
			if (queue.Count > 0)
			{
				Debug.LogError($"Need {queue.Count} more slot views");
			}
		}

		private void HandleSellButtonClicked(SlotSellView view)
		{
			this.OnSellModuleClicked?.Invoke(view);
		}

		private void Clear()
		{
			SlotSellView[] array = slotViews;
			foreach (SlotSellView obj in array)
			{
				obj.OnSlotViewPointerEnter -= HandleSlotPointerEnter;
				obj.OnSlotViewPointerExit -= HandleSlotPointerExit;
				obj.OnSellButtonClicked -= HandleSellButtonClicked;
			}
		}

		private void HandleSlotPointerEnter(SlotView view)
		{
			slotTooltip.Show(view.Slot);
		}

		private void HandleSlotPointerExit(SlotView view)
		{
			slotTooltip.Hide();
		}
	}
}
