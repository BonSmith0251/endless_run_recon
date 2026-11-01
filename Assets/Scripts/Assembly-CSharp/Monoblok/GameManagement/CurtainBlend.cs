using System;
using Monoblok.StateMachine;
using PrimeTween;

namespace Monoblok.GameManagement
{
	public class CurtainBlend : ICompletable
	{
		private readonly Curtain curtain;

		private readonly Action onFadeInComplete;

		private readonly float duration;

		private bool isComplete;

		public event Action OnComplete;

		public CurtainBlend(Curtain curtain, Action onFadeInComplete, float duration)
		{
			this.curtain = curtain;
			this.onFadeInComplete = onFadeInComplete;
			this.duration = duration;
		}

		public void Initialize()
		{
			isComplete = false;
			curtain.OnFadeComplete += HandleFadeComplete;
			curtain.FadeIn();
		}

		private void HandleFadeComplete()
		{
			if (!isComplete)
			{
				onFadeInComplete?.Invoke();
				Tween.Delay(this, duration, delegate(CurtainBlend t)
				{
					t.curtain.FadeOut();
				});
				isComplete = true;
			}
			else
			{
				curtain.OnFadeComplete -= HandleFadeComplete;
				this.OnComplete?.Invoke();
			}
		}
	}
}
