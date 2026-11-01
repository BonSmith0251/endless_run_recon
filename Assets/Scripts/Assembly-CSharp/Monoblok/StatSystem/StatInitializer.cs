using System;

namespace Monoblok.StatSystem
{
	[Serializable]
	public class StatInitializer
	{
		public StatDefinition StatDefinition;

		public ValueOrRuntimeValue<float> baseValue;

		public float BaseValue => baseValue.Value;

		public Stat GetStat()
		{
			return new Stat(StatDefinition, baseValue);
		}
	}
}
