using System.Linq;
using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Global Stat Modifier")]
	public class GlobalStatModifierModuleDefinition : ModuleDefinition
	{
		[SerializeField]
		private GlobalStatModifierModuleBehavior prefab;

		[SerializeField]
		private StatModifierDefinition[] modifierDefinitions;

		public override string Description
		{
			get
			{
				if (modifierDefinitions.Length == 0)
				{
					return base.Description;
				}
				return string.Join("; ", modifierDefinitions.Select((StatModifierDefinition m) => m.GetDescription()));
			}
		}

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			GlobalStatModifierModuleBehavior globalStatModifierModuleBehavior = Object.Instantiate(prefab, parent);
			globalStatModifierModuleBehavior.Initialize(base.Sprite, modifierDefinitions);
			return globalStatModifierModuleBehavior;
		}
	}
}
