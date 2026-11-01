using System;
using System.Collections.Generic;
using Monoblok.Core;
using UnityEngine;

namespace Monoblok.Weapon
{
	[Serializable]
	public class RandomTargetFilter : ITargetFilter
	{
		public Transform GetTarget(Transform origin, IList<Transform> candidates)
		{
			return candidates.GetRandom();
		}
	}
}
