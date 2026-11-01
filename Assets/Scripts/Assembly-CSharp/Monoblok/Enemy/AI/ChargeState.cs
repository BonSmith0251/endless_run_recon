using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class ChargeState : EnemyState
	{
		private readonly ChargeStateConfig config;

		private readonly Camera cam;

		private Vector2 origin;

		private float distance;

		private bool reached;

		public ChargeState(EnemyBrain brain, ChargeStateConfig config)
			: base(brain, config)
		{
			this.config = config;
			cam = Camera.main;
		}

		private Vector2 GetDestination()
		{
			float orthographicSize = cam.orthographicSize;
			float num = orthographicSize * cam.aspect;
			Vector3 position = cam.transform.position;
			float num2 = position.x + num;
			float num3 = position.x - num;
			float num4 = position.y + orthographicSize;
			float num5 = position.y - orthographicSize;
			Vector2 direction = config.Direction;
			Vector2 vector = brain.transform.position;
			float num6 = float.MaxValue;
			if (direction.x > 0f)
			{
				num6 = Mathf.Min(num6, (num2 - vector.x) / direction.x);
			}
			if (direction.x < 0f)
			{
				num6 = Mathf.Min(num6, (num3 - vector.x) / direction.x);
			}
			if (direction.y > 0f)
			{
				num6 = Mathf.Min(num6, (num4 - vector.y) / direction.y);
			}
			if (direction.y < 0f)
			{
				num6 = Mathf.Min(num6, (num5 - vector.y) / direction.y);
			}
			return vector + direction * (num6 + config.Padding);
		}

		protected override void OnEnter()
		{
			reached = false;
			origin = brain.transform.position;
			Vector2 destination = GetDestination();
			distance = Vector2.Distance(origin, destination);
			brain.Movement.SetDestination(new PositionTarget(destination));
		}

		protected override void OnExit()
		{
			brain.Movement.Stop();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (!reached && Vector2.Distance((Vector2)brain.transform.position, origin) >= distance)
			{
				Debug.Log("Charge distance reached");
				Finish();
				reached = true;
			}
		}
	}
}
