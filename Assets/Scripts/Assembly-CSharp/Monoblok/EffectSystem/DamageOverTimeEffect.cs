using Monoblok.DamageSystem;

namespace Monoblok.EffectSystem
{
	public class DamageOverTimeEffect : EffectInstance
	{
		private readonly float damagePerTick;

		private readonly float tickInterval;

		private float tickTimer;

		public DamageOverTimeEffect(EffectDefinition definition, object source, float damagePerTick, float tickInterval)
			: base(definition, source)
		{
			this.damagePerTick = damagePerTick;
			this.tickInterval = tickInterval;
		}

		public override void OnApply(EffectContext ctx)
		{
			tickTimer = 0f;
		}

		public override void OnUpdate(EffectContext ctx, float deltaTime)
		{
			tickTimer += deltaTime;
			while (tickTimer >= tickInterval)
			{
				tickTimer -= tickInterval;
				ctx.Health.Damage(new Damage((int)damagePerTick, 0, Damage.DamageType.DOT, bypassShield: true));
			}
		}

		public override void OnRemove(EffectContext ctx)
		{
		}
	}
}
