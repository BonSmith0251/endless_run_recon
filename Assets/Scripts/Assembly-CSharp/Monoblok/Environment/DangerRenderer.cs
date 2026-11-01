using UnityEngine;

namespace Monoblok.Environment
{
	public class DangerRenderer : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private Vector2 offset;

		private Transform target;

		private Camera mainCam;

		private Canvas canvas;

		private TrainTracks tracks;

		public Transform Target => target;

		private void Awake()
		{
			mainCam = Camera.main;
			canvas = GetComponentInParent<Canvas>();
		}

		public void Initialize(Transform target, TrainTracks tracks)
		{
			this.target = target;
			this.tracks = tracks;
		}

		private void LateUpdate()
		{
			if (target == null)
			{
				return;
			}
			Vector3 vector = mainCam.WorldToScreenPoint(tracks.transform.position);
			Vector3 vector2 = mainCam.WorldToScreenPoint(target.position);
			if (!(vector.z < 0f) && !(vector2.z < 0f))
			{
				Vector2 vector3 = vector;
				Vector2 vector4 = vector2;
				Vector2 vector5 = mainCam.WorldToScreenPoint(tracks.transform.position + tracks.transform.right) - vector;
				vector5.Normalize();
				float value = Vector2.Dot(vector4 - vector3, vector5);
				float num = 50f;
				float num2 = float.NegativeInfinity;
				float num3 = float.PositiveInfinity;
				if (Mathf.Abs(vector5.x) > 0.0001f)
				{
					float a = (num - vector3.x) / vector5.x;
					float b = ((float)Screen.width - num - vector3.x) / vector5.x;
					num2 = Mathf.Max(num2, Mathf.Min(a, b));
					num3 = Mathf.Min(num3, Mathf.Max(a, b));
				}
				if (Mathf.Abs(vector5.y) > 0.0001f)
				{
					float a2 = (num - vector3.y) / vector5.y;
					float b2 = ((float)Screen.height - num - vector3.y) / vector5.y;
					num2 = Mathf.Max(num2, Mathf.Min(a2, b2));
					num3 = Mathf.Min(num3, Mathf.Max(a2, b2));
				}
				value = Mathf.Clamp(value, num2, num3);
				Vector2 screenPoint = vector3 + vector5 * value;
				RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, screenPoint, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : mainCam, out var localPoint);
				rectTransform.anchoredPosition = localPoint + offset;
			}
		}
	}
}
