using Monoblok.SA;

namespace Monoblok.Upgrades
{
	public class SetRuntimeBoolValueUpgrade : IUpgrade
	{
		private readonly BoolRuntimeValue runtimeValue;

		private readonly bool targetState;

		public SetRuntimeBoolValueUpgrade(BoolRuntimeValue runtimeValue, bool targetState)
		{
			this.runtimeValue = runtimeValue;
			this.targetState = targetState;
		}

		public void Apply()
		{
			runtimeValue.SetValue(targetState);
		}

		public void Remove()
		{
			runtimeValue.SetValue(!targetState);
		}
	}
}
