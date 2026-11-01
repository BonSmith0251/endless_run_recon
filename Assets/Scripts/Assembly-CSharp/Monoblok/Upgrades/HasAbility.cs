using System;
using UnityEngine;

namespace Monoblok.Upgrades
{
	[Serializable]
	public class HasAbility : IRollEligibility
	{
		[SerializeField]
		private AbilityConfig ability;

		public bool IsElgible(ShopActionContext ctx)
		{
			return ctx.abilityManager.HasAbility(ability);
		}
	}
}
