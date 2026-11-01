using System;
using Monoblok.Movement;
using Monoblok.SA;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class MoveInCircleState : EnemyState
	{
		private const float LookaheadAngle = 0.34906584f;

		private const float OnCircleThreshold = 0.15f;

		private readonly TrainRuntimeValue trainRuntimeValue;

		private readonly MoveInCircleStateConfig config;

		private float radius;

		private PositionTarget positionTarget;

		private bool isOrbiting;

		private float previousAngle;

		private float accumulatedAngle;

		private float sweepAngle;

		public MoveInCircleState(EnemyBrain brain, MoveInCircleStateConfig config)
			: base(brain, config)
		{
			trainRuntimeValue = config.TrainRuntimeValue;
			this.config = config;
		}

		protected override void OnEnter()
		{
			radius = config.RadiusProvider.GetRadius();
			isOrbiting = false;
			accumulatedAngle = 0f;
			sweepAngle = UnityEngine.Random.Range(config.MinAngleDegrees, config.MaxAngleDegrees) * (MathF.PI / 180f);
			positionTarget = new PositionTarget(brain.transform.position);
			brain.Movement.SetDestination(positionTarget);
		}

		protected override void OnUpdate(float deltaTime)
		{
			Monoblok.Train.Train value = trainRuntimeValue.Value;
			if (value == null)
			{
				return;
			}
			Vector2 vector = value.Bounds.center;
			Vector2 vector2 = (Vector2)brain.transform.position - vector;
			float magnitude = vector2.magnitude;
			float num = ((magnitude > 0.0001f) ? Mathf.Atan2(vector2.y, vector2.x) : previousAngle);
			if (!isOrbiting)
			{
				if (Mathf.Abs(magnitude - radius) > 0.15f)
				{
					Vector2 vector3 = ((magnitude > 0.0001f) ? (vector2 / magnitude) : Vector2.right);
					positionTarget.UpdateTarget(vector + vector3 * radius);
					return;
				}
				isOrbiting = true;
				previousAngle = num;
				accumulatedAngle = 0f;
			}
			accumulatedAngle += Mathf.Abs(Mathf.DeltaAngle(previousAngle * 57.29578f, num * 57.29578f)) * (MathF.PI / 180f);
			previousAngle = num;
			float f = num + 0.34906584f;
			Vector2 vector4 = vector + new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * radius;
			positionTarget.UpdateTarget(vector4);
			if (!(accumulatedAngle < sweepAngle))
			{
				Finish();
			}
		}

		protected override void OnExit()
		{
			brain.Movement.Stop();
		}
	}
}
