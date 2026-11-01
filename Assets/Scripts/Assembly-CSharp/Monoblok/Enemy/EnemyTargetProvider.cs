using Monoblok.Movement;
using Monoblok.SA;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok.Enemy
{
	public class EnemyTargetProvider : MonoBehaviour
	{
		[SerializeField]
		private MovementBase movement;

		[SerializeField]
		private TrainRuntimeValue trainRuntimeValue;

		private TrainCart target;

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				FindTarget();
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
					float num2 = Vector2.Distance((Vector2)cart.Center.position, (Vector2)base.transform.position);
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
				movement.SetDestination(new TransformTarget(trainCart.Center));
			}
		}
	}
}
