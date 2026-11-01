using System;

namespace Monoblok.DamageSystem
{
	public readonly struct Damage
	{
		[Flags]
		public enum DamageType
		{
			Undefined = 0,
			Contact = 1,
			Projectile = 2,
			DOT = 4
		}

		private readonly DamageType damageType;

		private readonly int baseDmg;

		private readonly int bonusDamage;

		public DamageType Type => damageType;

		public int DamageValue => baseDmg + bonusDamage;

		public bool IsCrit => bonusDamage != 0;

		public int Addition => bonusDamage;

		public bool BypassShield { get; }

		public Damage(int baseDamage, int addition, DamageType damageType, bool bypassShield = false)
		{
			if (baseDamage < 0)
			{
				throw new ArgumentOutOfRangeException("baseDamage");
			}
			this.damageType = damageType;
			baseDmg = baseDamage;
			bonusDamage = addition;
			BypassShield = bypassShield;
		}

		public override string ToString()
		{
			return DamageValue.ToString();
		}
	}
}
