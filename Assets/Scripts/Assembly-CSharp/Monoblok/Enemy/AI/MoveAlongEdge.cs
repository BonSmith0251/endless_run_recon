using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class MoveAlongEdge : EnemyState
	{
		private readonly GameArea gameArea;

		private readonly MoveAlongEdgeConfig.Edge edge;

		private readonly int dir;

		private readonly float padding;

		private Vector2 startPos;

		private Vector2 endPos;

		private bool reachedStart;

		public MoveAlongEdge(EnemyBrain brain, MoveAlongEdgeConfig config)
			: base(brain, config)
		{
			gameArea = config.GameArea;
			edge = config.EdgeToMoveAlong;
			dir = config.Direction;
			padding = config.Padding;
		}

		protected override void OnEnter()
		{
			CalculateEdgePositions();
			reachedStart = false;
			brain.Movement.SetDestination(new PositionTarget(startPos));
		}

		protected override void OnExit()
		{
			brain.Movement.Stop();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (!reachedStart)
			{
				if (!(Vector2.Distance((Vector2)brain.transform.position, startPos) > 0.1f))
				{
					reachedStart = true;
					brain.Movement.SetDestination(new PositionTarget(endPos));
				}
			}
			else if (!(Vector2.Distance((Vector2)brain.transform.position, endPos) > 0.1f))
			{
				Finish();
			}
		}

		private void CalculateEdgePositions()
		{
			Bounds bounds = gameArea.Bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			switch (edge)
			{
			case MoveAlongEdgeConfig.Edge.Top:
			{
				float y2 = vector2.y - ClampedPadding(vector.y, vector2.y);
				float num4 = ClampedPadding(vector.x, vector2.x, 0.5f);
				startPos = new Vector2(vector.x + num4, y2);
				endPos = new Vector2(vector2.x - num4, y2);
				break;
			}
			case MoveAlongEdgeConfig.Edge.Bottom:
			{
				float y = vector.y + ClampedPadding(vector.y, vector2.y);
				float num3 = ClampedPadding(vector.x, vector2.x, 0.5f);
				startPos = new Vector2(vector.x + num3, y);
				endPos = new Vector2(vector2.x - num3, y);
				break;
			}
			case MoveAlongEdgeConfig.Edge.Left:
			{
				float x2 = vector.x + ClampedPadding(vector.x, vector2.x);
				float num2 = ClampedPadding(vector.y, vector2.y, 0.5f);
				startPos = new Vector2(x2, vector.y + num2);
				endPos = new Vector2(x2, vector2.y - num2);
				break;
			}
			case MoveAlongEdgeConfig.Edge.Right:
			{
				float x = vector2.x - ClampedPadding(vector.x, vector2.x);
				float num = ClampedPadding(vector.y, vector2.y, 0.5f);
				startPos = new Vector2(x, vector.y + num);
				endPos = new Vector2(x, vector2.y - num);
				break;
			}
			}
			if (dir == 0)
			{
				Vector2 vector3 = endPos;
				Vector2 vector4 = startPos;
				startPos = vector3;
				endPos = vector4;
			}
		}

		private float ClampedPadding(float min, float max, float maxRatio = 1f)
		{
			return Mathf.Min(padding, (max - min) * maxRatio);
		}
	}
}
