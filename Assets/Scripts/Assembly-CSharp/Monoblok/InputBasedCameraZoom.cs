using Monoblok.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace Monoblok
{
	public class InputBasedCameraZoom : MonoBehaviour
	{
		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private CinemachineCamera cam;

		[SerializeField]
		private float zoomOutSize = 8f;

		[SerializeField]
		private float smoothTime = 0.25f;

		[SerializeField]
		private float inputThreshold = 0.1f;

		private float originalZoom;

		private float zoomVelocity;

		private void Awake()
		{
			originalZoom = cam.Lens.OrthographicSize;
		}

		private void Update()
		{
			float target = ((Mathf.Abs(inputReader.TrainMove) > inputThreshold) ? zoomOutSize : originalZoom);
			float orthographicSize = Mathf.SmoothDamp(cam.Lens.OrthographicSize, target, ref zoomVelocity, smoothTime);
			LensSettings lens = cam.Lens;
			lens.OrthographicSize = orthographicSize;
			cam.Lens = lens;
		}
	}
}
