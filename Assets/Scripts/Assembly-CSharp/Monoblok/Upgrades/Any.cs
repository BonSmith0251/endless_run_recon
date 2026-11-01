using System;
using UnityEngine;
using ZLinq;

namespace Monoblok.Upgrades
{
	[Serializable]
	public class Any : IRollEligibility
	{
		[SerializeReference]
		[SubclassSelector]
		private IRollEligibility[] inner;

		public bool IsElgible(ShopActionContext ctx)
		{
			return inner.AsValueEnumerable().Any((IRollEligibility e) => e.IsElgible(ctx));
		}
	}
}
