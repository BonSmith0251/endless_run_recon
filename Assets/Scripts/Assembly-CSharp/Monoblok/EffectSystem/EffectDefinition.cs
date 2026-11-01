using System;
using Monoblok.Weapon;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.EffectSystem
{
	public abstract class EffectDefinition : ScriptableObject
	{
		[field: SerializeField]
		public float Duration { get; private set; } = 5f;


		[field: SerializeField]
		[field: Expandable]
		public EffectVisualData VisualData { get; private set; }

		[field: SerializeReference]
		[field: SubclassSelector]
		public IStackingStrategy StackingStrategy { get; private set; }
	}
	public abstract class EffectDefinition<TPayload> : EffectDefinition
	{
		public abstract Func<TPayload> RegisterStats(WeaponStatBuilder builder);

		public abstract void UnregisterStats(WeaponStatBuilder builder);

		public abstract EffectInstance CreateInstance(object source, TPayload payload);
	}
}
