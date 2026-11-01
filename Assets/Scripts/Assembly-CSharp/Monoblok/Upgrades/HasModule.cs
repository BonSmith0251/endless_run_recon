using System;
using Monoblok.Train;
using UnityEngine;
using ZLinq;

namespace Monoblok.Upgrades
{
	[Serializable]
	public class HasModule : IRollEligibility
	{
		[SerializeField]
		private ModuleDefinition module;

		public bool IsElgible(ShopActionContext ctx)
		{
			return (from m in ctx.train.Carts.AsValueEnumerable().SelectMany((TrainCart c) => c.ModuleManager.Modules)
				where m.Definition == module
				select m).Any();
		}
	}
}
