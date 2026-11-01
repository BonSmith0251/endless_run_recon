using PrimeTween;
using UnityEngine;

namespace Monoblok.GameManagement
{
	[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
	public class StationView : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private TweenSettings<float> showSettings;

		[SerializeField]
		private TweenSettings<float> hideSettings;

		private void OnValidate()
		{
			if (canvasGroup == null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		private void Awake()
		{
			Hide();
		}

		public void Show()
		{
			canvasGroup.alpha = 1f;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			Tween.UIAnchoredPositionX(rectTransform, showSettings);
		}

		public Tween Hide()
		{
			return Tween.UIAnchoredPositionX(rectTransform, hideSettings).OnComplete(this, delegate(StationView t)
			{
				t.canvasGroup.alpha = 0f;
				t.canvasGroup.interactable = false;
				t.canvasGroup.blocksRaycasts = false;
			});
		}
	}
}
