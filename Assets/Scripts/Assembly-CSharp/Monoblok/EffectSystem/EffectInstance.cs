namespace Monoblok.EffectSystem
{
	public abstract class EffectInstance
	{
		public readonly object Source;

		public readonly EffectDefinition EffectDefinition;

		public float RemainingTime;

		public EffectInstance(EffectDefinition effectDefinition, object source)
		{
			EffectDefinition = effectDefinition;
			RemainingTime = effectDefinition.Duration;
			Source = source;
		}

		public abstract void OnApply(EffectContext ctx);

		public virtual void OnUpdate(EffectContext ctx, float deltaTime)
		{
		}

		public abstract void OnRemove(EffectContext ctx);
	}
}
