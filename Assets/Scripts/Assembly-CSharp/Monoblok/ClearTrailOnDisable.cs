using UnityEngine;

namespace Monoblok
{
	public class ClearTrailOnDisable : MonoBehaviour
	{
		[SerializeField]
		private TrailRenderer trailRenderer;

		private void OnValidate()
		{
			if (!(trailRenderer != null))
			{
				trailRenderer = GetComponent<TrailRenderer>();
			}
		}

		private void OnDisable()
		{
			if (!(trailRenderer == null))
			{
				trailRenderer.Clear();
			}
		}
	}
}
