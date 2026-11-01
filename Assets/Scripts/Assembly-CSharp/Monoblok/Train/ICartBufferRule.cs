using Monoblok.StatSystem;

namespace Monoblok.Train
{
	public interface ICartBufferRule
	{
		ITrainCartTargetProvider TargetProvider { get; }

		StatDefinition StatDefinition { get; }

		StatModifierFactory ModifierFactory { get; }

		float MaxValue { get; }

		bool BuffOrDebuff { get; }
	}
}
