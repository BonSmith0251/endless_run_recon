using System;
using UnityEngine;

namespace Monoblok.SA
{
	public abstract class GenericGameEvent<T> : ScriptableObject
	{
		public event Action<T> OnTriggered;

		public void TriggerGameEvent(T value)
		{
			this.OnTriggered?.Invoke(value);
		}
	}
}
