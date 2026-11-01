using System;
using Monoblok.DamageSystem;

namespace Monoblok.Enemy.AI
{
	public class ExplodeState : EnemyState
	{
		private readonly int damage;

		private readonly float radius;

		private readonly int layer;

		private EnemyExplode explode;

		public ExplodeState(EnemyBrain brain, ExplodeStateConfig config)
			: base(brain, config)
		{
			explode = brain.GetComponent<EnemyExplode>();
			if (explode == null)
			{
				throw new InvalidOperationException("No EnemyExplode component found on enemy");
			}
			damage = config.Damage;
			radius = config.Radius;
			layer = config.Layer;
		}

		protected override void OnEnter()
		{
			explode.Explode(new Damage(damage, 0, Damage.DamageType.Undefined), layer, radius);
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
