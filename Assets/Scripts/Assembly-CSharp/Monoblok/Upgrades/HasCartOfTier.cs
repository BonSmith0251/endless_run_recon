using System;
using System.Linq;
using Monoblok.Train;
using UnityEngine;
using ZLinq;

namespace Monoblok.Upgrades
{
	[Serializable]
	public class HasCartOfTier : IRollEligibility
	{
		[SerializeField]
		private CartTier[] tiers;

		public bool IsElgible(ShopActionContext ctx)
		{
			return ctx.train.Carts.AsValueEnumerable().Any((TrainCart c) => tiers.Contains(c.CartData.Tier));
		}
	}
}
