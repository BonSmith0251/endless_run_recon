using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public abstract class LandConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public float Delay { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new Land(brain, this);
		}

		public abstract Vector2 GetTarget();
	}
}
