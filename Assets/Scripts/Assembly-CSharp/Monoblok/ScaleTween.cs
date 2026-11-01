using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	public class ScaleTween : MonoBehaviour
	{
		[SerializeField]
		private TweenSettings<float> scaleUpSettings;

		[SerializeField]
		private TweenSettings<float> scaleDownSettings;

		public void ScaleUp()
		{
			Tween.Scale(base.transform, scaleUpSettings);
		}

		public void ScaleDown()
		{
			Tween.Scale(base.transform, scaleDownSettings);
		}
	}
}
