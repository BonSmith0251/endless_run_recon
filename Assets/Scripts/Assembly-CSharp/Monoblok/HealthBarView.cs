using Monoblok.DamageSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasGroup))]
	public class HealthBarView : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private Slider slider;

		[SerializeField]
		private Vector2 offset;

		private Canvas canvas;

		private RectTransform canvasRect;

		private Health health;

		public CanvasGroup CanvasGroup => canvasGroup;

		private void OnValidate()
		{
			if (rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();
			}
			if (canvasGroup == null)
			{
				canvasGroup = GetComponent<CanvasGroup>();
			}
		}

		private void Awake()
		{
			canvas = GetComponentInParent<Canvas>().rootCanvas;
			canvasRect = canvas.GetComponent<RectTransform>();
		}

		public void Initialize(Health health)
		{
			this.health = health;
			health.OnHealthChanged += OnHealthChanged;
			UpdateSlider();
		}

		private void OnDestroy()
		{
			if (!(health == null))
			{
				health.OnHealthChanged -= OnHealthChanged;
			}
		}

		private void OnHealthChanged(int _)
		{
			UpdateSlider();
		}

		private void UpdateSlider()
		{
			if (!(slider == null))
			{
				slider.value = (float)health.CurrentHealth / (float)health.MaxHealth;
			}
		}

		public void UpdatePosition(Camera cam)
		{
			Bounds bounds = health.BoundsProvider.GetBounds();
			Vector3 position = bounds.center + new Vector3(0f, bounds.size.y / 2f, 0f);
			Vector3 vector = cam.WorldToScreenPoint(position);
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, vector, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : cam, out var localPoint))
			{
				rectTransform.localPosition = localPoint + offset;
			}
		}
	}
}
