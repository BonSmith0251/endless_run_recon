using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Explode")]
	public class ExplodeStateConfig : EnemyStateConfig
	{
		[field: SerializeField]
		public int Damage { get; private set; }

		[field: SerializeField]
		public float Radius { get; private set; }

		[field: SerializeField]
		[field: Layer]
		public int Layer { get; private set; }

		public override EnemyState GetState(EnemyBrain brain)
		{
			return new ExplodeState(brain, this);
		}
	}
}
