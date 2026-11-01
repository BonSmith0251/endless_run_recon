using System;
using System.Collections.Generic;

namespace Monoblok.EffectSystem
{
	[Serializable]
	public class UniquePerSourceStrategy : IStackingStrategy
	{
		public StackResult TryApply(EffectDefinition definition, IEnumerable<EffectInstance> activeEffects, EffectContext ctx, object source)
		{
			foreach (EffectInstance activeEffect in activeEffects)
			{
				if (!(activeEffect.EffectDefinition != definition) && activeEffect.Source == source)
				{
					return StackResult.Stacked;
				}
			}
			return StackResult.None;
		}
	}
}
