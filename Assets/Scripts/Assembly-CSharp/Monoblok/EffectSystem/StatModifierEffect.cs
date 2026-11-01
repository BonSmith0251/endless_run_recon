using System;
using Monoblok.StatSystem;

namespace Monoblok.EffectSystem
{
	public class StatModifierEffect : EffectInstance
	{
		private readonly StatDefinition statDefinition;

		private readonly Func<EffectInstance, StatModifier> modifierFactory;

		private StatModifier modifier;

		public StatModifierEffect(EffectDefinition effectDefinition, object source, StatDefinition statDefinition, Func<EffectInstance, StatModifier> modifierFactory)
			: base(effectDefinition, source)
		{
			this.statDefinition = statDefinition;
			this.modifierFactory = modifierFactory;
		}

		public override void OnApply(EffectContext ctx)
		{
			if (modifier == null && ctx.StatManager.TryGetStat(statDefinition, out var stat))
			{
				modifier = modifierFactory(this);
				stat.AddModifier(modifier);
			}
		}

		public override void OnRemove(EffectContext ctx)
		{
			if (ctx.StatManager.TryGetStat(statDefinition, out var stat))
			{
				stat.RemoveModifiersFromSource(this);
			}
		}
	}
}
