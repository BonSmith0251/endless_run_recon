namespace Monoblok.EffectSystem
{
	public abstract class EffectVisual
	{
		public abstract void Apply(EffectHandler target);

		public abstract void Remove(EffectHandler target);
	}
}
