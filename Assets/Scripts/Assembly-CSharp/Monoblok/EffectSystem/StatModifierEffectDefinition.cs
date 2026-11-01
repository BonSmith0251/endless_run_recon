using System;
using Monoblok.StatSystem;
using Monoblok.Weapon;
using UnityEngine;

namespace Monoblok.EffectSystem
{
	[CreateAssetMenu(menuName = "Game/Effect/Stat Modifier")]
	public class StatModifierEffectDefinition : EffectDefinition<float>
	{
		public enum ModifierType
		{
			Multiplicative,
			Additive
		}

		[SerializeField]
		private StatDefinition statDefinition;

		[SerializeField]
		private ModifierType type;

		[SerializeField]
		private StatInitializer magnitude;

		[Tooltip("Multiplicative only. Applies (1 - magnitude) instead of magnitude. Use for potency-style stats (e.g. slow amount) where higher magnitude should mean a stronger effect, so generic stat-boosting upgrades make the effect stronger instead of weaker.")]
		[SerializeField]
		private bool invertMultiplier;

		public override Func<float> RegisterStats(WeaponStatBuilder builder)
		{
			builder.Add(magnitude, out var magnitudeStat);
			return () => magnitudeStat.Value;
		}

		public override void UnregisterStats(WeaponStatBuilder builder)
		{
			builder.Remove(magnitude);
		}

		public override EffectInstance CreateInstance(object source, float magnitude)
		{
			return new StatModifierEffect(this, source, statDefinition, (EffectInstance s) => CreateModifier(s, magnitude));
		}

		private StatModifier CreateModifier(object source, float magnitude)
		{
			return type switch
			{
				ModifierType.Multiplicative => new MultiplicativeModifier(source, invertMultiplier ? (1f - magnitude) : magnitude), 
				ModifierType.Additive => new AdditiveModifier(source, magnitude), 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}
	}
}
