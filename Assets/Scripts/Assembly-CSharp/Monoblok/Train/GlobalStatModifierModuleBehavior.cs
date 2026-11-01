using System.Collections.Generic;
using Monoblok.SA;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Train
{
	public class GlobalStatModifierModuleBehavior : ModuleBehavior
	{
		[SerializeField]
		private GlobalStatManagerRuntimeValue statManagerValue;

		private IEnumerable<StatModifierDefinition> modifierDefinitions;

		private List<Stat> modifiedStats = new List<Stat>();

		public void Initialize(Sprite sprite, IEnumerable<StatModifierDefinition> modifierDefinitions)
		{
			this.modifierDefinitions = modifierDefinitions;
			spriteRenderer.sprite = sprite;
		}

		protected override void OnModuleEnabled()
		{
			modifiedStats.Clear();
			foreach (StatModifierDefinition modifierDefinition in modifierDefinitions)
			{
				if (modifierDefinition != null && statManagerValue.Value.StatManager.TryGetStat(modifierDefinition.statDefinition, out var stat))
				{
					float magnitude = modifierDefinition.modifierFactory.GetMagnitude();
					StatModifier scaledModifier = modifierDefinition.modifierFactory.GetScaledModifier(this, magnitude);
					modifiedStats.Add(stat);
					stat.AddModifier(scaledModifier);
				}
			}
		}

		protected override void OnModuleDisabled()
		{
			foreach (Stat modifiedStat in modifiedStats)
			{
				modifiedStat?.RemoveModifiersFromSource(this);
			}
			modifiedStats.Clear();
		}
	}
}
