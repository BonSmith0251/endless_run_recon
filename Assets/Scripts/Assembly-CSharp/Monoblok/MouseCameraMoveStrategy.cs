using Monoblok.Input;
using UnityEngine;

namespace Monoblok
{
	public class MouseCameraMoveStrategy : ICameraMoveStrategy
	{
		private readonly Camera camera;

		private readonly InputReader inputReader;

		private readonly float dragSensitivity;

		private readonly float followDepth;

		private bool isDragging;

		private Vector3 dragStartWorldPos;

		private Vector3 targetStartPos;

		public MouseCameraMoveStrategy(Camera camera, InputReader inputReader, float dragSensitivity, float followDepth)
		{
			this.camera = camera;
			this.inputReader = inputReader;
			this.dragSensitivity = dragSensitivity;
			this.followDepth = followDepth;
		}

		public Vector3? GetDesiredPosition(Vector3 currentPosition)
		{
			if (inputReader.PointerPressedThisFrame)
			{
				isDragging = true;
				dragStartWorldPos = GetPointerWorldPosition();
				targetStartPos = currentPosition;
			}
			if (inputReader.PointerReleasedThisFrame)
			{
				isDragging = false;
			}
			if (!isDragging)
			{
				return null;
			}
			Vector3 vector = dragStartWorldPos - GetPointerWorldPosition();
			return targetStartPos + vector * dragSensitivity;
		}

		private Vector3 GetPointerWorldPosition()
		{
			Vector3 position = inputReader.PointerPosition;
			position.z = followDepth;
			return camera.ScreenToWorldPoint(position);
		}
	}
}
