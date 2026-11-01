using System.Collections.Generic;
using UnityEngine;

namespace Monoblok
{
	public class AbilityManagerView : MonoBehaviour
	{
		[SerializeField]
		private AbilitySlotView slotViewPrefab;

		[SerializeField]
		private RectTransform slotContainer;

		private List<AbilitySlotView> slotViews = new List<AbilitySlotView>();

		public void RegisterSlot(AbilitySlot slot)
		{
			AbilitySlotView abilitySlotView = Object.Instantiate(slotViewPrefab, slotContainer);
			abilitySlotView.Initialize(slot);
			slotViews.Add(abilitySlotView);
		}
	}
}
