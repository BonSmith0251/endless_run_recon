using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class MoveToGameAreaNormalized : EnemyState
	{
		private readonly GameArea area;

		private readonly Vector2 normalizedPos;

		private Vector2 targetPos;

		public MoveToGameAreaNormalized(EnemyBrain brain, MoveToGameAreaNormalizedConfig config)
			: base(brain, config)
		{
			area = config.GameArea;
			normalizedPos = config.NormalizedPosition;
		}

		protected override void OnEnter()
		{
			Bounds bounds = area.Bounds;
			targetPos = new Vector2(Mathf.Lerp(bounds.min.x, bounds.max.x, normalizedPos.x), Mathf.Lerp(bounds.min.y, bounds.max.y, normalizedPos.y));
			brain.Movement.SetDestination(new PositionTarget(targetPos));
		}

		protected override void OnExit()
		{
			brain.Movement.Stop();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (!(Vector2.Distance((Vector2)brain.transform.position, targetPos) > 0.1f))
			{
				Finish();
			}
		}
	}
}
