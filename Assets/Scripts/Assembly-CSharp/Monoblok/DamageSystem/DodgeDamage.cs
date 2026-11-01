using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class DodgeDamage : MonoBehaviour, IStatManagerInjectable
	{
		[SerializeField]
		private StatOrValue dodgeChance;

		public bool Dodge()
		{
			if (dodgeChance.Value == 0f)
			{
				return false;
			}
			if (Random.value <= (float)dodgeChance)
			{
				return true;
			}
			return false;
		}

		public void Inject(StatManager statManager)
		{
			dodgeChance.Bind(statManager);
		}
	}
}
