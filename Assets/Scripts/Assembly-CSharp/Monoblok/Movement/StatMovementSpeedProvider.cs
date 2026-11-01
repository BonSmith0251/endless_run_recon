using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Movement
{
	public class StatMovementSpeedProvider : MovementSpeedProvider
	{
		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		private StatDefinition definition;

		public override float GetSpeed()
		{
			return statManager.GetStatValue(definition);
		}
	}
}
