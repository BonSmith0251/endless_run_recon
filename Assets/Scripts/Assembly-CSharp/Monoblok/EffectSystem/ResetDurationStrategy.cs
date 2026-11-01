using System;
using System.Collections.Generic;

namespace Monoblok.EffectSystem
{
	[Serializable]
	public class ResetDurationStrategy : IStackingStrategy
	{
		public StackResult TryApply(EffectDefinition definition, IEnumerable<EffectInstance> activeEffects, EffectContext ctx, object source)
		{
			foreach (EffectInstance activeEffect in activeEffects)
			{
				if (!(activeEffect.EffectDefinition != definition))
				{
					activeEffect.RemainingTime = definition.Duration;
					return StackResult.Stacked;
				}
			}
			return StackResult.None;
		}
	}
}
