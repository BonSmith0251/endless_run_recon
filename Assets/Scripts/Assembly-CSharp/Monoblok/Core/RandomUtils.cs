using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monoblok.Core
{
	public static class RandomUtils
	{
		public static T GetRandom<T>(this T[] elements)
		{
			if (elements == null || elements.Length == 0)
			{
				return default(T);
			}
			return elements[Random.Range(0, elements.Length)];
		}

		public static T GetRandom<T>(this IList<T> elements)
		{
			if (elements == null || elements.Count == 0)
			{
				return default(T);
			}
			return elements[Random.Range(0, elements.Count)];
		}

		public static T GetRandom<T>(this IEnumerable<T> elements)
		{
			if (elements == null)
			{
				return default(T);
			}
			IList<T> list = (elements as IList<T>) ?? elements.ToList();
			if (list.Count == 0)
			{
				return default(T);
			}
			return list[Random.Range(0, list.Count)];
		}

		public static float GetRandomInRange(this Vector2 range)
		{
			return Random.Range(range.x, range.y);
		}

		public static int GetRandomInRange(this Vector2Int range)
		{
			return Random.Range(range.x, range.y + 1);
		}
	}
}
