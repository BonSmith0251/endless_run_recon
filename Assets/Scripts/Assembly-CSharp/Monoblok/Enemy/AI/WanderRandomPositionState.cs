using System;
using Monoblok.Movement;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class WanderRandomPositionState : EnemyState
	{
		private readonly WanderRandomPositionStateConfig config;

		private Vector2 destination;

		public WanderRandomPositionState(EnemyBrain brain, WanderRandomPositionStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			Vector2? vector = FindRandomPosition();
			if (!vector.HasValue)
			{
				Finish();
				return;
			}
			destination = vector.Value;
			brain.Movement.SetDestination(new PositionTarget(destination));
		}

		protected override void OnExit()
		{
			brain.Movement.Stop();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (Vector2.Distance((Vector2)brain.transform.position, destination) <= config.ArrivalDistance)
			{
				Finish();
			}
		}

		private Vector2? FindRandomPosition()
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return null;
			}
			_ = main.orthographicSize;
			_ = main.aspect;
			Vector2 vector = main.transform.position;
			Vector2 vector2 = brain.transform.position;
			float f = UnityEngine.Random.Range(0f, 360f) * (MathF.PI / 180f);
			Vector2 vector3 = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * config.WanderRadius;
			Vector2 vector4 = vector2 + vector3;
			Vector2 normalized = (vector - vector4).normalized;
			return vector4 + normalized * UnityEngine.Random.Range(config.MinDistance, config.MaxDistance);
		}
	}
}
