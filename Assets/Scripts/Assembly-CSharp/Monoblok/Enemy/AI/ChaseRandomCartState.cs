using Monoblok.Movement;
using Monoblok.SA;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class ChaseRandomCartState : EnemyState
	{
		private readonly TrainRuntimeValue trainRuntimeValue;

		private readonly float stoppingDistance;

		private TrainCart target;

		public ChaseRandomCartState(EnemyBrain brain, ChaseRandomCartStateConfig config)
			: base(brain, config)
		{
			trainRuntimeValue = config.TrainRuntimeValue;
			stoppingDistance = config.StoppingDistance;
		}

		protected override void OnEnter()
		{
			FindTarget();
		}

		protected override void OnExit()
		{
			brain.Movement.Stop();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (!(target == null) && !(Vector2.Distance((Vector2)brain.transform.position, (Vector2)target.transform.position) > stoppingDistance))
			{
				Finish();
			}
		}

		private void FindTarget()
		{
			Monoblok.Train.Train value = trainRuntimeValue.Value;
			if (value == null)
			{
				return;
			}
			float num = float.MaxValue;
			TrainCart trainCart = null;
			foreach (TrainCart cart in value.Carts)
			{
				if (!(cart == null))
				{
					float num2 = Vector2.Distance((Vector2)cart.Center.position, (Vector2)brain.transform.position);
					if (!(num2 >= num))
					{
						trainCart = cart;
						num = num2;
					}
				}
			}
			if (!(trainCart == null))
			{
				target = trainCart;
				brain.Movement.SetDestination(new TransformTarget(trainCart.Center));
			}
		}
	}
}
