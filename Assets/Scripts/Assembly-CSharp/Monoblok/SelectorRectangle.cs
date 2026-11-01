using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	public class SelectorRectangle : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private Canvas canvas;

		[SerializeField]
		private Image image;

		[SerializeField]
		private float screenPadding;

		[SerializeField]
		private TweenSettings<float> scaleSettings;

		private RectTransform canvasRect;

		private Camera mainCam;

		private SpriteRenderer target;

		private void Awake()
		{
			Hide();
			canvasRect = canvas.GetComponent<RectTransform>();
			mainCam = Camera.main;
		}

		public void Show(SpriteRenderer renderer)
		{
			target = renderer;
			UpdatePositionAndSize();
			image.enabled = true;
			Tween.Scale(base.transform, scaleSettings);
		}

		private void LateUpdate()
		{
			UpdatePositionAndSize();
		}

		private void UpdatePositionAndSize()
		{
			if (!(target == null))
			{
				Bounds bounds = target.bounds;
				Vector3 vector = mainCam.WorldToScreenPoint(bounds.center);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, vector, canvas.worldCamera, out var localPoint);
				rectTransform.anchoredPosition = localPoint;
				Vector3 vector2 = mainCam.WorldToScreenPoint(bounds.min);
				Vector3 vector3 = mainCam.WorldToScreenPoint(bounds.max);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, vector2, null, out var localPoint2);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, vector3, null, out var localPoint3);
				Vector2 vector4 = localPoint3 - localPoint2;
				image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, vector4.x + screenPadding);
				image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vector4.y + screenPadding);
			}
		}

		[Button(null, EButtonEnableMode.Always)]
		public void Hide()
		{
			image.enabled = false;
			Tween.StopAll(base.transform);
			target = null;
		}
	}
}
