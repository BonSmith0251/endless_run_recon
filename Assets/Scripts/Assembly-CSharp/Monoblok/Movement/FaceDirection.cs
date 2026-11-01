using UnityEngine;

namespace Monoblok.Movement
{
	public class FaceDirection : MonoBehaviour
	{
		private enum Direction
		{
			Left,
			Right
		}

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		[Min(0f)]
		private float deadZone = 0.1f;

		private Direction dir;

		private float lastX;

		private void Awake()
		{
			lastX = base.transform.position.x;
		}

		private void Update()
		{
			float num = base.transform.position.x - lastX;
			if (!(Mathf.Abs(num) < deadZone))
			{
				if (num > 0f)
				{
					SetDirection(Direction.Right);
				}
				else if (num < 0f)
				{
					SetDirection(Direction.Left);
				}
				lastX = base.transform.position.x;
			}
		}

		private void SetDirection(Direction dir)
		{
			if (this.dir != dir)
			{
				this.dir = dir;
				spriteRenderer.flipX = dir == Direction.Right;
			}
		}
	}
}
