using System;
using UnityEngine;

namespace Monoblok.Progression
{
	public abstract class WaveData : ScriptableObject
	{
		public event Action OnStarted;

		public event Action OnEnded;

		public void Initialize()
		{
			OnStart();
			this.OnStarted?.Invoke();
		}

		public void End()
		{
			this.OnEnded?.Invoke();
			OnEnd();
			Debug.Log("Wave ended");
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnEnd()
		{
		}
	}
}
