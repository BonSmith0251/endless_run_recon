using Monoblok.Isometric;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Land On Rails")]
	public class LandOnRailsConfig : LandConfig
	{
		[SerializeField]
		private float range;

		public override Vector2 GetTarget()
		{
			Vector2 vector = (((double)Random.value >= 0.5) ? Iso.Left : Iso.Right);
			return Vector2.zero + vector * Random.Range(0f, range / 2f);
		}
	}
}
