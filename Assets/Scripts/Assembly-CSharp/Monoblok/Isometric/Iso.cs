using UnityEngine;

namespace Monoblok.Isometric
{
	public static class Iso
	{
		public static readonly Vector2 Right = new Vector2(1f, -0.5f).normalized;

		public static readonly Vector2 Left = new Vector2(-1f, 0.5f).normalized;

		public static readonly Vector2 Up = new Vector2(1f, 0.5f).normalized;

		public static readonly Vector2 Down = new Vector2(-1f, -0.5f).normalized;
	}
}
