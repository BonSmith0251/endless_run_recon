using UnityEngine;

namespace Monoblok.Enemy
{
	public abstract class SpawnPatternConfig : ScriptableObject
	{
		[SerializeReference]
		[SubclassSelector]
		private ISpawnAnchor anchor;

		public SpawnPoint[] GetSpawnPoints(Camera cam, Transform origin, int count)
		{
			Vector3 vector = anchor.GetAnchor(cam, origin);
			return GetSpawnPoints(vector, count);
		}

		protected abstract SpawnPoint[] GetSpawnPoints(Vector3 anchor, int count);
	}
}
