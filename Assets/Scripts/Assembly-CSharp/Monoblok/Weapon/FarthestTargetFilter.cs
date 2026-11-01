using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.Weapon
{
	[Serializable]
	public class FarthestTargetFilter : ITargetFilter
	{
		public Transform GetTarget(Transform origin, IList<Transform> candidates)
		{
			Transform result = null;
			float num = float.MinValue;
			for (int i = 0; i < candidates.Count; i++)
			{
				Transform transform = candidates[i];
				if (!(transform == null))
				{
					float sqrMagnitude = ((Vector2)transform.position - (Vector2)origin.position).sqrMagnitude;
					if (!(sqrMagnitude <= num))
					{
						num = sqrMagnitude;
						result = transform;
					}
				}
			}
			return result;
		}
	}
}
