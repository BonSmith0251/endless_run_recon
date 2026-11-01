using Monoblok.SA;
using Monoblok.StatSystem;
using Monoblok.Upgrades;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Cart Trade/Config/Global Stat Modifier")]
	public class GlobalStatModifierTradeRewardConfig : CartTradeRewardConfig
	{
		private static readonly LocalizedString descriptionTemplate = new LocalizedString("Trade", "stat_line");

		[field: SerializeField]
		public GlobalStatManagerRuntimeValue GlobalStatModifierValue { get; private set; }

		[field: SerializeField]
		public StatDefinition StatDefinition { get; private set; }

		[field: SerializeField]
		public StatModifierFactory ModifierFactory { get; private set; }

		public override string Description => descriptionTemplate.GetLocalizedString(new
		{
			value = ModifierFactory.GetColoredString(),
			stat = StatDefinition.Name
		});

		public override CartTradeReward GetReward(Price basePrice)
		{
			return new GlobalStatModifierTradeReward(this, basePrice);
		}
	}
}
