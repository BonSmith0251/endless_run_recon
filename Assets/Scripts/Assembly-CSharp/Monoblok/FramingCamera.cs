using Monoblok.SA;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

namespace Monoblok
{
	public class FramingCamera : MonoBehaviour
	{
		[SerializeField]
		private CinemachineCamera cam;

		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		[SerializeField]
		private SpriteRenderer station;

		[SerializeField]
		private Padding contentPadding;

		[SerializeField]
		private float orthoPadding;

		private Camera mainCam;

		private Bounds bounds;

		public CinemachineCamera Camera => cam;

		private void Awake()
		{
			mainCam = UnityEngine.Camera.main;
		}

		public void FrameCamera(RectTransform safeAreaUI, bool instant = true)
		{
			if (mainCam == null)
			{
				mainCam = UnityEngine.Camera.main;
			}
			bounds = trainRuntimeValue.Value.Bounds;
			bounds.Encapsulate(station.bounds);
			bounds.min -= new Vector3(contentPadding.left, contentPadding.bottom, 0f);
			bounds.max += new Vector3(contentPadding.right, contentPadding.top, 0f);
			float aspect = cam.Lens.Aspect;
			Rect safeViewportRect = GetSafeViewportRect(safeAreaUI);
			float x = bounds.size.x;
			float a = bounds.size.y / (2f * safeViewportRect.height);
			float b = x / (2f * safeViewportRect.width * aspect);
			float num = Mathf.Max(a, b);
			Vector3 center = bounds.center;
			Vector2 center2 = safeViewportRect.center;
			float num2 = num * 2f;
			float num3 = num2 * aspect;
			Vector3 vector = new Vector3((0.5f - center2.x) * num3, (0.5f - center2.y) * num2, 0f);
			Vector3 pos = center + vector;
			pos.z = cam.transform.position.z;
			SetValues(num, pos, instant);
		}

		private Rect GetSafeViewportRect(RectTransform safeAreaUI)
		{
			Vector3[] array = new Vector3[4];
			safeAreaUI.GetWorldCorners(array);
			Vector3 position = array[0];
			Vector3 position2 = array[2];
			Vector3 vector = mainCam.ScreenToViewportPoint(position);
			Vector3 vector2 = mainCam.ScreenToViewportPoint(position2);
			return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
		}

		private void SetValues(float orthosSize, Vector3 pos, bool instant)
		{
			if (instant)
			{
				cam.Lens.OrthographicSize = orthosSize + orthoPadding;
				cam.transform.position = pos;
				return;
			}
			Tween.Custom(this, cam.Lens.OrthographicSize, orthosSize + orthoPadding, 0.15f, delegate(FramingCamera t, float v)
			{
				t.cam.Lens.OrthographicSize = v;
			});
			Tween.Position(cam.transform, pos, 0.15f);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}
	}
}
