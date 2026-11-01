using UnityEngine;

namespace Monoblok
{
	[CreateAssetMenu(menuName = "Game/Abilities/Screen Wipe")]
	public class ScreenWipeAbilityConfig : AbilityConfig
	{
		[field: SerializeField]
		public ScreenWiper Prefab { get; private set; }

		public override Ability GetAbility()
		{
			return new ScreenWipeAbility(this);
		}
	}
}
