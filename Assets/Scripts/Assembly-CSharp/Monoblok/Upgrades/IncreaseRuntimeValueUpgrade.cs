using Monoblok.SA;

namespace Monoblok.Upgrades
{
	public class IncreaseRuntimeValueUpgrade : IUpgrade
	{
		private readonly RuntimeValue<float> runtimeValue;

		private readonly float step;

		public IncreaseRuntimeValueUpgrade(RuntimeValue<float> runtimeValue, float step)
		{
			this.runtimeValue = runtimeValue;
			this.step = step;
		}

		public void Apply()
		{
			float value = runtimeValue.Value + step;
			runtimeValue.SetValue(value);
		}

		public void Remove()
		{
			float value = runtimeValue.Value - step;
			runtimeValue.SetValue(value);
		}
	}
}
