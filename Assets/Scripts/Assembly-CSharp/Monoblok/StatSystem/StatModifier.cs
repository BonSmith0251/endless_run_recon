namespace Monoblok.StatSystem
{
	public abstract class StatModifier
	{
		public object Source { get; }

		public abstract int Order { get; }

		protected StatModifier(object source)
		{
			Source = source;
		}

		public abstract float Modify(float value);
	}
}
