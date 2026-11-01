using UnityEngine;

namespace Monoblok.Core
{
	public static class TransformUtils
	{
		public static void Clear(this Transform transform)
		{
			if (transform == null)
			{
				return;
			}
			for (int num = transform.childCount - 1; num >= 0; num--)
			{
				Transform child = transform.GetChild(num);
				if (!(child == null))
				{
					Object.Destroy(child.gameObject);
				}
			}
		}
	}
}
