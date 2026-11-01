using Monoblok.Input;
using UnityEngine;

namespace Monoblok
{
	public class GamepadCameraMoveStrategy : ICameraMoveStrategy
	{
		private readonly InputReader inputReader;

		private readonly float moveSpeed;

		public GamepadCameraMoveStrategy(InputReader inputReader, float moveSpeed)
		{
			this.inputReader = inputReader;
			this.moveSpeed = moveSpeed;
		}

		public Vector3? GetDesiredPosition(Vector3 currentPosition)
		{
			Vector2 cameraMove = inputReader.CameraMove;
			if (cameraMove == Vector2.zero)
			{
				return null;
			}
			return currentPosition + new Vector3(cameraMove.x, cameraMove.y, 0f) * (moveSpeed * Time.deltaTime);
		}
	}
}
