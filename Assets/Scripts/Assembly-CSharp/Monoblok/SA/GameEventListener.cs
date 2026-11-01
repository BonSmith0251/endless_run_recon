using UltEvents;
using UnityEngine;

namespace Monoblok.SA
{
	public class GameEventListener : MonoBehaviour
	{
		[SerializeField]
		private GameEvent gameEvent;

		[SerializeField]
		private UltEvent response;

		private void OnEnable()
		{
			if (Application.isPlaying && !(gameEvent == null))
			{
				gameEvent.OnTriggered += OnGameEventTriggered;
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying && !(gameEvent == null))
			{
				gameEvent.OnTriggered -= OnGameEventTriggered;
			}
		}

		private void OnGameEventTriggered()
		{
			response?.Invoke();
		}
	}
}
