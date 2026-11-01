using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monoblok.Environment
{
	public class DangerManager : MonoBehaviour
	{
		[SerializeField]
		private DangerRenderer rendererPrefab;

		[SerializeField]
		private RectTransform container;

		private List<DangerRenderer> renderers = new List<DangerRenderer>();

		public void RegisterDanger(Transform target, TrainTracks tracks)
		{
			DangerRenderer dangerRenderer = Object.Instantiate(rendererPrefab, container);
			dangerRenderer.Initialize(target, tracks);
			renderers.Add(dangerRenderer);
		}

		public void RemoveDanger(Transform target)
		{
			DangerRenderer dangerRenderer = renderers.FirstOrDefault((DangerRenderer r) => r.Target == target);
			if (!(dangerRenderer == null))
			{
				Object.Destroy(dangerRenderer.gameObject);
			}
		}
	}
}
