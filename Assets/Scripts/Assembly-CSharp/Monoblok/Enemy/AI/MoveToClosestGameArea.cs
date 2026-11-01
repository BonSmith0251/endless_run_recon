using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class MoveToClosestGameArea : EnemyState
	{
		private readonly MoveToClosestGameAreaConfig config;

		private Vector3 targetPos;

		public MoveToClosestGameArea(EnemyBrain brain, MoveToClosestGameAreaConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			GameArea gameArea = null;
			float num = float.MaxValue;
			GameArea[] gameAreas = config.GameAreas;
			foreach (GameArea gameArea2 in gameAreas)
			{
				float num2 = Vector2.Distance((Vector2)brain.transform.position, (Vector2)gameArea2.Bounds.center);
				if (num2 < num)
				{
					num = num2;
					gameArea = gameArea2;
				}
			}
			if (gameArea == null)
			{
				Finish();
				return;
			}
			targetPos = gameArea.GetRandomPosition();
			brain.Movement.SetDestination(new PositionTarget(targetPos));
		}

		protected override void OnExit()
		{
			brain.Movement.Stop();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (!(Vector2.Distance((Vector2)brain.transform.position, (Vector2)targetPos) > 0.1f))
			{
				Finish();
			}
		}
	}
}
