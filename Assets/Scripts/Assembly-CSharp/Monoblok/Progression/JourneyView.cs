using UnityEngine;

namespace Monoblok.Progression
{
	public class JourneyView : MonoBehaviour
	{
		[SerializeField]
		private Progression progression;

		[SerializeField]
		private JourneyStageView[] stageViews;

		private void Awake()
		{
			progression.OnJourneyStarted += HandleJourneyStarted;
			progression.OnInitialized += ResetViews;
		}

		private void OnDestroy()
		{
			progression.OnJourneyStarted -= HandleJourneyStarted;
			progression.OnInitialized -= ResetViews;
		}

		private void HandleJourneyStarted()
		{
			int currentWaveIndex = progression.CurrentWaveIndex;
			for (int i = 0; i <= currentWaveIndex && i < stageViews.Length; i++)
			{
				stageViews[i].ToggleOn();
			}
		}

		private void ResetViews()
		{
			JourneyStageView[] array = stageViews;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ToggleOff();
			}
		}
	}
}
