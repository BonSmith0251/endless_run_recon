using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class MoveToGameArea : EnemyState
	{
		private readonly MoveToGameAreaConfig config;

		private Vector3 targetPos;

		public MoveToGameArea(EnemyBrain brain, MoveToGameAreaConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			targetPos = config.GameArea.GetRandomPosition();
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
