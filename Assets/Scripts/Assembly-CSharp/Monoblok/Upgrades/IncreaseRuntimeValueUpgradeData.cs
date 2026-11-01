using Monoblok.Core;
using Monoblok.SA;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Upgrades
{
	[CreateAssetMenu(menuName = "Game/Upgrade/Increase Runtime Value")]
	public class IncreaseRuntimeValueUpgradeData : UpgradeData
	{
		private static readonly LocalizedString descriptionTemplate = new LocalizedString("Upgrades", "value_line");

		[SerializeField]
		[Expandable]
		private FloatRuntimeValue runtimeValue;

		[SerializeField]
		private float step;

		[SerializeField]
		private DescriptionFormatting.ValueFormat valueFormat;

		[SerializeField]
		private LocalizedString statName;

		public override string Description => descriptionTemplate.GetLocalizedString(new
		{
			value = DescriptionFormatting.GetColoredValue(step, valueFormat),
			stat = statName.GetLocalizedString()
		});

		public override IUpgrade GetUpgrade()
		{
			return new IncreaseRuntimeValueUpgrade(runtimeValue, step);
		}
	}
}
