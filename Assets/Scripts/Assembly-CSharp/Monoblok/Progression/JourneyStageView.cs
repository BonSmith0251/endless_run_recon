using PrimeTween;
using UnityEngine;

namespace Monoblok.Progression
{
	public class JourneyStageView : MonoBehaviour
	{
		[SerializeField]
		private GameObject fill;

		private bool isOn;

		private void Awake()
		{
			fill.transform.localScale = Vector3.zero;
			isOn = false;
		}

		public void ToggleOn()
		{
			if (!isOn)
			{
				Tween.Scale(fill.transform, 1f, 0.15f, Ease.OutBack);
				isOn = true;
			}
		}

		public void ToggleOff()
		{
			if (isOn)
			{
				fill.transform.localScale = Vector3.zero;
				isOn = false;
			}
		}
	}
}
