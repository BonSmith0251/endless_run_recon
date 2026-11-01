using UnityEngine;

namespace Monoblok
{
	public class ScreenWipeAbility : Ability
	{
		private ScreenWiper prefab;

		public ScreenWipeAbility(ScreenWipeAbilityConfig config)
			: base(config)
		{
			prefab = config.Prefab;
		}

		protected override void Execute()
		{
			Object.Instantiate(prefab).WipeScreen();
		}
	}
}
