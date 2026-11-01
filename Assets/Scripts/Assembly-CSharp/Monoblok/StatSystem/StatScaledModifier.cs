namespace Monoblok.StatSystem
{
	public class StatScaledModifier : StatModifier
	{
		private readonly StatModifier _inner;

		private readonly Stat _bonusStat;

		public override int Order => _inner.Order;

		public StatScaledModifier(object source, StatModifier inner, Stat bonusStat)
			: base(source)
		{
			_inner = inner;
			_bonusStat = bonusStat;
		}

		public override float Modify(float value)
		{
			float num = _inner.Modify(value) - value;
			return value + num * _bonusStat.Value;
		}
	}
}
