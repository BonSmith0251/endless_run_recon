using PrimeTween;
using TMPro;
using UnityEngine;

namespace Monoblok.Train
{
	[RequireComponent(typeof(RectTransform))]
	public class MergeConnectionView : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private TMP_Text remainingWavesText;

		[SerializeField]
		private Vector2 offset = new Vector2(0f, 50f);

		private CartMergeConnection connection;

		private Canvas canvas;

		private RectTransform canvasRect;

		private void OnValidate()
		{
			if (!(rectTransform != null))
			{
				rectTransform = GetComponent<RectTransform>();
			}
		}

		private void Awake()
		{
			canvas = GetComponentInParent<Canvas>().rootCanvas;
			canvasRect = canvas.GetComponent<RectTransform>();
		}

		public void Initialize(CartMergeConnection connection)
		{
			this.connection = connection;
			connection.WavesRemaining.OnValueChanged += UpdateRemainingWaves;
			UpdateRemainingWaves(connection.WavesRemaining.Value);
		}

		private void UpdateRemainingWaves(int remainingWaves)
		{
			remainingWavesText.text = remainingWaves.ToString();
			Tween.PunchScale(base.transform, Vector3.one * 1.2f, 0.2f, 5f);
		}

		public void UpdatePosition(Camera cam)
		{
			Vector3 position = connection.Child.transform.position;
			Vector3 vector = cam.WorldToScreenPoint(position);
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, vector, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : cam, out var localPoint))
			{
				rectTransform.localPosition = localPoint + offset;
			}
		}
	}
}
