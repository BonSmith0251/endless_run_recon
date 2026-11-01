using Monoblok.StatSystem;

namespace Monoblok.Upgrades
{
	public class ModifyGlobalStatReward : LevelupReward
	{
		private readonly GlobalStatManager globalStatManager;

		private readonly StatModifierDefinition modifierDefinition;

		private Stat modifiedStat;

		public ModifyGlobalStatReward(ModifyGlobalStatRewardConfig config)
			: base(config)
		{
			globalStatManager = config.StatManagerValue.Value;
			modifierDefinition = config.ModifierDefinition;
		}

		public override void Gain()
		{
			if (globalStatManager.StatManager.TryGetStat(modifierDefinition.statDefinition, out var stat))
			{
				StatModifier modifier = modifierDefinition.modifierFactory.GetModifier(this);
				stat.AddModifier(modifier);
				modifiedStat = stat;
			}
		}

		public override void Lose()
		{
			if (modifiedStat != null)
			{
				modifiedStat.RemoveModifiersFromSource(this);
				modifiedStat = null;
			}
		}
	}
}
