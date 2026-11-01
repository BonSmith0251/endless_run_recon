using Monoblok.Core;
using Monoblok.SA;
using Monoblok.Upgrades;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Cart Trade/Config/Increase Runtime Value")]
	public class IncreaseRuntimeValueRewardConfig : CartTradeRewardConfig
	{
		private static readonly LocalizedString descriptionTemplate = new LocalizedString("Trade", "stat_line");

		[SerializeField]
		private DescriptionFormatting.ValueFormat valueFormat;

		[SerializeField]
		private LocalizedString statName;

		[field: SerializeField]
		public FloatRuntimeValue RuntimeValue { get; private set; }

		[field: SerializeField]
		public float Step { get; private set; }

		public override string Description => descriptionTemplate.GetLocalizedString(new
		{
			value = DescriptionFormatting.GetColoredValue(Step, valueFormat),
			stat = statName.GetLocalizedString()
		});

		public override CartTradeReward GetReward(Price basePrice)
		{
			return new IncreaseRuntimeValueReward(this, basePrice);
		}
	}
}
