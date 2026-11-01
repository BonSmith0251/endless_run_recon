using Monoblok.StatSystem;
using Monoblok.Train;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	[RequireComponent(typeof(RectTransform))]
	public class SlotTooltip : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private Image moduleImage;

		[SerializeField]
		private TMP_Text moduleNameText;

		[SerializeField]
		private TMP_Text moduleDescriptionText;

		[SerializeField]
		private StatCollectionView statCollectionView;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		[HideIf("ignoreSourcePosition")]
		private Vector2 offset;

		private void OnValidate()
		{
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		public void Show(ModuleSlot slot)
		{
			if (slot != null && slot.ModuleData != null)
			{
				base.gameObject.SetActive(value: true);
				moduleImage.sprite = slot.ModuleData.Definition.Sprite;
				moduleNameText.text = slot.ModuleData.Definition.DisplayName;
				moduleDescriptionText.text = slot.ModuleData.Definition.Description;
				statCollectionView.Initialize(slot.Behavior.StatManager.StatMap.Values);
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
