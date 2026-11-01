using System;
using UnityEngine;

namespace Monoblok
{
	public abstract class AchievementSink
	{
		private readonly SteamTrackable trackable;

		public event Action<AchievementSink> OnReadyToDispose;

		public AchievementSink(SteamTrackable trackable)
		{
			this.trackable = trackable;
		}

		public bool TryInitialize()
		{
			if (trackable == null)
			{
				Debug.LogError("Trackable is not set");
				return false;
			}
			if (!trackable.ShouldTrack())
			{
				return false;
			}
			OnInitialize();
			Debug.Log("Sink initialized");
			return true;
		}

		public void Dispose()
		{
			OnDispose();
		}

		protected abstract void OnInitialize();

		protected abstract void OnDispose();

		protected void Notify()
		{
			if (trackable == null)
			{
				Debug.LogError("Trackable is not set");
				return;
			}
			trackable.Trigger(out var achieved);
			if (achieved)
			{
				this.OnReadyToDispose?.Invoke(this);
			}
		}
	}
}
