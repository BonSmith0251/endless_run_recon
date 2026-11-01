using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Progression
{
	public class ProgressionSlider : MonoBehaviour
	{
		[SerializeField]
		private TimedWaveData data;

		[SerializeField]
		private Slider slider;

		[SerializeField]
		private LayoutElement layoutElement;

		private void Awake()
		{
			slider.transform.localScale = new Vector3(0f, 1f, 1f);
			layoutElement.ignoreLayout = true;
		}

		private void OnEnable()
		{
			data.OnStarted += Open;
			data.OnEnded += Close;
		}

		private void OnDisable()
		{
			data.OnStarted -= Open;
			data.OnEnded -= Close;
		}

		private void Update()
		{
			UpdateProgress(data.NormalizedTime);
		}

		public void Open()
		{
			layoutElement.ignoreLayout = false;
			Tween.ScaleX(slider.transform, 1f, 0.3f, Ease.OutBack);
		}

		public void Close()
		{
			Tween.ScaleX(slider.transform, 0f, 0.3f, Ease.InBack).OnComplete(this, delegate(ProgressionSlider t)
			{
				t.layoutElement.ignoreLayout = true;
			});
		}

		public void UpdateProgress(float normalizedProgress)
		{
			if (!(slider == null))
			{
				slider.value = normalizedProgress;
			}
		}
	}
}
