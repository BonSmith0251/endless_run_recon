using System;
using UnityEngine.Localization;

namespace Monoblok.StatSystem
{
	[Serializable]
	public class StatModifierDefinition
	{
		private static readonly LocalizedString descriptionTemplate = new LocalizedString("Modules", "global_line");

		public StatDefinition statDefinition;

		public StatModifierFactory modifierFactory;

		public string GetDescription()
		{
			return descriptionTemplate.GetLocalizedString(new
			{
				value = modifierFactory.GetColoredString(),
				stat = statDefinition.Name
			});
		}
	}
}
