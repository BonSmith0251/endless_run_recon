using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Upgrades
{
	[RequireComponent(typeof(RectTransform))]
	public class UpgradeNodeTooltip : MonoBehaviour
	{
		[SerializeField]
		private Camera worldCamera;

		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private Image nodeIcon;

		[SerializeField]
		private TMP_Text nodeNameText;

		[SerializeField]
		private TMP_Text nodeDescriptionText;

		[SerializeField]
		private RuntimePriceView priceView;

		[Header("Config")]
		[SerializeField]
		private Vector2 offset;

		[SerializeField]
		private TweenSettings<float> showScaleSettings;

		[SerializeField]
		private TweenSettings<float> hideScaleSettings;

		[SerializeField]
		private ShakeSettings showShakeSettings;

		private UpgradeNodeView currentTarget;

		private void OnValidate()
		{
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		public void Initialize(UpgradeTreeNodeInstance node)
		{
			if (!(node.UpgradeData == null))
			{
				SetIcon(node.UpgradeData.Sprite);
				SetName(node.UpgradeData.UpgradeName);
				SetDescription(node.UpgradeData.Description);
				SetPrice(node.Price);
			}
		}

		private void SetIcon(Sprite icon)
		{
			if (!(nodeIcon == null))
			{
				nodeIcon.sprite = icon;
			}
		}

		private void SetName(string name)
		{
			if (!(nodeNameText == null))
			{
				nodeNameText.text = name;
			}
		}

		private void SetDescription(string description)
		{
			if (!(nodeDescriptionText == null))
			{
				nodeDescriptionText.text = description;
			}
		}

		private void SetPrice(RuntimePrice price)
		{
			if (!(priceView == null) && price != null && !(price.Currency == null))
			{
				priceView.Initialize(price);
			}
		}

		public void Show(UpgradeNodeView view)
		{
			currentTarget = view;
			Sequence.Create().Chain(Tween.Scale(base.transform, showScaleSettings)).Group(Tween.ShakeLocalRotation(base.transform, showShakeSettings));
		}

		private void Update()
		{
			if (!(currentTarget == null) && !(worldCamera == null))
			{
				Vector3 position = currentTarget.transform.position;
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(worldCamera, position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, screenPoint, worldCamera, out var localPoint);
				rectTransform.anchoredPosition = localPoint + offset;
			}
		}

		public void Hide()
		{
			currentTarget = null;
			Tween.Scale(base.transform, hideScaleSettings);
		}
	}
}
