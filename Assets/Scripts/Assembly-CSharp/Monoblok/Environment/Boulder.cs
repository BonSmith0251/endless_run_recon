using Monoblok.Isometric;
using UnityEngine;

namespace Monoblok.Environment
{
	public class Boulder : EnvironmentMember
	{
		private Vector3 startPos;

		private float spawnWorldDistance;

		public void Initialize(Vector3 position, TrainTracks tracks, float currentWorldDistance)
		{
			base.transform.position = position;
			startPos = position;
			spawnWorldDistance = currentWorldDistance;
		}

		public override void UpdateEnvironmentMember(float worldDistance, float normalizedSpeed)
		{
			float num = worldDistance - spawnWorldDistance;
			base.transform.position = startPos + num * (Vector3)Iso.Left;
		}
	}
}
