using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public abstract class EnemyStateConfig : ScriptableObject
	{
		[field: SerializeField]
		public AnimationClip EnterAnimation { get; private set; }

		[field: SerializeField]
		public StatModifierDefinition[] ModifierDefinitions { get; private set; }

		public abstract EnemyState GetState(EnemyBrain brain);
	}
}
