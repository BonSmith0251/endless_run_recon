using Monoblok.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace Monoblok
{
	public class ZoomCamera : MonoBehaviour
	{
		[SerializeField]
		private CinemachineCamera virtualCam;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		[Min(0.01f)]
		private float sensitivity = 5f;

		[SerializeField]
		[Min(0.01f)]
		private float minZoom = 2f;

		[SerializeField]
		[Min(0.01f)]
		private float maxZoom = 20f;

		[SerializeField]
		private float smoothSpeed = 10f;

		private float targetZoom;

		private void Awake()
		{
			targetZoom = virtualCam.Lens.OrthographicSize;
		}

		private void Update()
		{
			Vector2 scroll = inputReader.Scroll;
			if (scroll.y != 0f)
			{
				targetZoom -= scroll.y * sensitivity;
				targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
			}
			virtualCam.Lens.OrthographicSize = Mathf.Lerp(virtualCam.Lens.OrthographicSize, targetZoom, Time.deltaTime * smoothSpeed);
		}
	}
}
