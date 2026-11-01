using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Set Weapon State")]
	public class SetWeaponStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public bool EnableWeapon { get; private set; }

		[field: SerializeField]
		public Vector2 Direction { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new SetWeaponState(brain, this);
		}
	}
}
