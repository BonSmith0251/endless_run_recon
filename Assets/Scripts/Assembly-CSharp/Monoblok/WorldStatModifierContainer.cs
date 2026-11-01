using Monoblok.StatSystem;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	[RequireComponent(typeof(RectTransform))]
	public class WorldStatModifierContainer : MonoBehaviour
	{
		[SerializeField]
		private WorldStatModifierView viewPrefab;

		private RectTransform rectTransform;

		private Camera mainCam;

		private RectTransform canvasRect;

		private TrainCart target;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			mainCam = Camera.main;
			canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
		}

		public void Initialize(TrainCart target)
		{
			this.target = target;
		}

		public void AddView(StatDefinition def, bool buffOrDebuff)
		{
			Object.Instantiate(viewPrefab, base.transform).Initialize(def, buffOrDebuff);
		}

		private void LateUpdate()
		{
			if (!(target == null))
			{
				Vector3 vector = mainCam.WorldToScreenPoint(target.Center.position);
				RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, vector, null, out var localPoint);
				rectTransform.anchoredPosition = localPoint;
			}
		}
	}
}
