namespace Monoblok.StatSystem
{
	public class MultiplicativeModifier : StatModifier
	{
		public readonly float Multiplier;

		public override int Order => 1;

		public MultiplicativeModifier(object source, float multiplier)
			: base(source)
		{
			Multiplier = multiplier;
		}

		public override float Modify(float value)
		{
			return value * Multiplier;
		}
	}
}
