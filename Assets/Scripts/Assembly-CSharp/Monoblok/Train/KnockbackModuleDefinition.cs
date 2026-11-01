using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Knockback")]
	public class KnockbackModuleDefinition : ModuleDefinition
	{
		[SerializeField]
		private KnockbackModuleBehavior modulePrefab;

		[SerializeField]
		private float radius;

		[SerializeField]
		private float cooldownTime = 1f;

		[SerializeField]
		private StatInitializer damage;

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			KnockbackModuleBehavior knockbackModuleBehavior = Object.Instantiate(modulePrefab, parent);
			knockbackModuleBehavior.Initialize(base.Sprite, radius, cooldownTime, damage);
			return knockbackModuleBehavior;
		}
	}
}
