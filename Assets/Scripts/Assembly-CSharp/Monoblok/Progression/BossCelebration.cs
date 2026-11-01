using System;
using Animancer;
using FMODUnity;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Progression
{
	public class BossCelebration : MonoBehaviour
	{
		[SerializeField]
		private AnimancerComponent animancer;

		[SerializeField]
		private AnimationClip openClip;

		[SerializeField]
		private AnimationClip endClip;

		[SerializeField]
		private ParticleSystem ps;

		[SerializeField]
		private EventReference eventRef;

		private AnimancerState animationState;

		private Tween delayTween;

		public event Action OnComplete;

		private void Awake()
		{
			EnableContainer(enable: false);
		}

		[Button(null, EButtonEnableMode.Always)]
		public void PlayAnimation()
		{
			animancer.Stop();
			ps.Stop();
			delayTween.Stop();
			EnableContainer(enable: true);
			RuntimeManager.PlayOneShot(eventRef);
			animationState = animancer.Play(openClip);
			animationState.Time = 0f;
			animationState.OwnedEvents.EndEvent.callback = OnAnimationFinished;
		}

		private void OnAnimationFinished()
		{
			animationState.Stop();
			ps.Play();
			delayTween = Tween.Delay(2f).OnComplete(this, delegate(BossCelebration t)
			{
				t.OnPsComplete();
			});
		}

		private void OnPsComplete()
		{
			animancer.Play(endClip).Time = 0f;
			delayTween = Tween.Delay(2f).OnComplete(this, delegate(BossCelebration t)
			{
				t.EnableContainer(enable: false);
			});
			this.OnComplete?.Invoke();
		}

		private void EnableContainer(bool enable)
		{
			animancer.gameObject.SetActive(enable);
		}

		public void Cancel()
		{
			animancer.Stop();
			ps.Stop();
			delayTween.Stop();
			EnableContainer(enable: false);
		}
	}
}
