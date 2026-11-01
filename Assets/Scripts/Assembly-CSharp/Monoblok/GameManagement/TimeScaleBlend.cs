using System;
using Monoblok.StateMachine;
using PrimeTween;

namespace Monoblok.GameManagement
{
	public class TimeScaleBlend : ICompletable
	{
		private readonly float target;

		private readonly float duration;

		public event Action OnComplete;

		public TimeScaleBlend(float timeScaleTarget, float duration = 1f)
		{
			target = timeScaleTarget;
			this.duration = duration;
		}

		public void Initialize()
		{
			Tween.GlobalTimeScale(target, duration).OnComplete(this, delegate(TimeScaleBlend t)
			{
				t.OnComplete?.Invoke();
			});
		}
	}
}
