using TMPro;
using UnityEngine;

namespace Monoblok.Progression
{
	public class WaveCounterView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text counterText;

		[SerializeField]
		private Progression progression;

		[SerializeField]
		private string prefix;

		private void OnEnable()
		{
			progression.OnJourneyStarted += UpdateText;
			progression.OnInitialized += UpdateText;
			UpdateText();
		}

		private void OnDisable()
		{
			progression.OnJourneyStarted -= UpdateText;
			progression.OnInitialized -= UpdateText;
		}

		private void UpdateText()
		{
			counterText.text = prefix + " " + (progression.CurrentWaveIndex + 1);
		}
	}
}
