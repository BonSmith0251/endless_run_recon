using System.Collections.Generic;

namespace Monoblok.EffectSystem
{
	public interface IStackingStrategy
	{
		StackResult TryApply(EffectDefinition definition, IEnumerable<EffectInstance> activeEffects, EffectContext ctx, object source);
	}
}
