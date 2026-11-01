using UltEvents;
using UnityEngine;

namespace Monoblok.Progression
{
	public class ProgressionEvents : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private Progression progression;

		[SerializeField]
		private UltEvent OnJourneyStarted;

		[SerializeField]
		private UltEvent OnJourneyEnded;

		[SerializeField]
		private UltEvent OnWaveStarted;

		[SerializeField]
		private UltEvent OnWaveEnded;

		private void OnEnable()
		{
			progression.OnJourneyStarted += HandleOnJourneyStarted;
			progression.OnJourneyEnded += HandleOnJourneyEnded;
			progression.OnWaveStarted += HandleOnWaveStarted;
			progression.OnWaveEnded += HandleOnWaveEnded;
		}

		private void OnDisable()
		{
			progression.OnJourneyStarted -= HandleOnJourneyStarted;
			progression.OnJourneyEnded -= HandleOnJourneyEnded;
			progression.OnWaveStarted -= HandleOnWaveStarted;
			progression.OnWaveEnded -= HandleOnWaveEnded;
		}

		private void Start()
		{
			OnJourneyEnded?.Invoke();
		}

		private void HandleOnJourneyStarted()
		{
			OnJourneyStarted?.Invoke();
		}

		private void HandleOnJourneyEnded()
		{
			OnJourneyEnded?.Invoke();
		}

		private void HandleOnWaveStarted()
		{
			OnWaveStarted?.Invoke();
		}

		private void HandleOnWaveEnded()
		{
			OnWaveEnded?.Invoke();
		}
	}
}
