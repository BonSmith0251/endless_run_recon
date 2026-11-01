using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.Weapon
{
	public interface ITargetFilter
	{
		Transform GetTarget(Transform origin, IList<Transform> candidates);
	}
}
