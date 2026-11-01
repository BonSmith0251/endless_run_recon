using System;
using UnityEngine;

namespace Monoblok.SA
{
	[CreateAssetMenu(menuName = "Game/SA/Game Event")]
	public class GameEvent : ScriptableObject
	{
		public event Action OnTriggered;

		public void TriggerGameEvent()
		{
			this.OnTriggered?.Invoke();
		}
	}
}
