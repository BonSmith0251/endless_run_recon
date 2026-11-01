namespace Monoblok.StatSystem
{
	public class AdditiveModifier : StatModifier
	{
		public readonly float Addition;

		public override int Order => 0;

		public AdditiveModifier(object source, float addition)
			: base(source)
		{
			Addition = addition;
		}

		public override float Modify(float value)
		{
			return value + Addition;
		}
	}
}
