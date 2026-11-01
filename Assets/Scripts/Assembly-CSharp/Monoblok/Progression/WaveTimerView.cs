using TMPro;
using UnityEngine;

namespace Monoblok.Progression
{
	public class WaveTimerView : MonoBehaviour
	{
		[SerializeField]
		private TimedWaveData data;

		[SerializeField]
		private TMP_Text timerText;

		private int lastDisplayedSeconds = -1;

		private void Awake()
		{
			ResetTimerText();
		}

		private void OnEnable()
		{
			data.OnEnded += HandleWaveEnded;
		}

		private void OnDisable()
		{
			data.OnEnded -= HandleWaveEnded;
		}

		private void HandleWaveEnded()
		{
			ResetTimerText();
		}

		public void ResetTimerText()
		{
			lastDisplayedSeconds = -1;
			UpdateTimer(0f);
		}

		private void Update()
		{
			if (!(data == null))
			{
				UpdateTimer(data.RemainingTime);
			}
		}

		public void UpdateTimer(float time)
		{
			int num = (int)time;
			if (num != lastDisplayedSeconds)
			{
				lastDisplayedSeconds = num;
				timerText.text = FormatTime(num);
			}
		}

		private string FormatTime(int totalSeconds)
		{
			int num = totalSeconds / 60;
			int num2 = totalSeconds % 60;
			return num + ":" + num2.ToString("D2");
		}
	}
}
