using Monoblok.Input;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

namespace Monoblok
{
	public class DragCamera : MonoBehaviour
	{
		[SerializeField]
		private Camera targetCamera;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private CinemachineCamera virtualCam;

		[SerializeField]
		private float dragSensitivity = 1f;

		[SerializeField]
		private float gamepadMoveSpeed = 5f;

		private Transform followTarget;

		private ICameraMoveStrategy strategy;

		private bool hasBounds;

		private Bounds bounds;

		public float GamepadMoveSpeed => gamepadMoveSpeed;

		public float DragSensitivity => dragSensitivity;

		public Camera TargetCamera => targetCamera;

		public InputReader InputReader => inputReader;

		private void Awake()
		{
			if (virtualCam.Follow == null)
			{
				GameObject gameObject = new GameObject("CameraFollowTarget");
				gameObject.transform.position = virtualCam.transform.position;
				virtualCam.Follow = gameObject.transform;
			}
			followTarget = virtualCam.Follow;
		}

		private void Update()
		{
			if (strategy != null)
			{
				Vector3? desiredPosition = strategy.GetDesiredPosition(followTarget.position);
				if (desiredPosition.HasValue)
				{
					ApplyPosition(desiredPosition.Value);
				}
			}
		}

		public void SetStrategy(ICameraMoveStrategy newStrategy)
		{
			strategy = newStrategy;
		}

		public void ForcePosition(Vector3 position, bool instant = false)
		{
			if (!(followTarget == null))
			{
				float duration = (instant ? 0f : 0.3f);
				Vector3 position2 = followTarget.position;
				Tween.Custom(followTarget, position2, position, duration, delegate(Transform target, Vector3 val)
				{
					ApplyPosition(val);
				});
			}
		}

		public void SetBounds(Bounds bounds)
		{
			this.bounds = bounds;
			hasBounds = true;
		}

		private void ApplyPosition(Vector3 position)
		{
			position.z = followTarget.position.z;
			if (hasBounds)
			{
				position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
				position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);
			}
			followTarget.position = position;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}
	}
}
