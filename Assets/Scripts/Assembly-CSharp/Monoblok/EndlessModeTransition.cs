using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok
{
	public class EndlessModeTransition : MonoBehaviour
	{
		[SerializeField]
		private Animator animator;

		private HashSet<IEndlessModeBlendable> blendables = new HashSet<IEndlessModeBlendable>();

		private static readonly int TransitionHash = Animator.StringToHash("transition");

		public event Action OnTransitionComplete;

		[Button(null, EButtonEnableMode.Always)]
		public void StartTransition()
		{
			animator.SetTrigger(TransitionHash);
		}

		public void ExecuteBlend()
		{
			foreach (IEndlessModeBlendable blendable in blendables)
			{
				blendable?.Blend();
			}
		}

		public void UndoBlend()
		{
			foreach (IEndlessModeBlendable blendable in blendables)
			{
				blendable.UnBlend();
			}
		}

		public void CompleteTransition()
		{
			this.OnTransitionComplete?.Invoke();
		}

		public void Register(IEndlessModeBlendable blendable)
		{
			blendables.Add(blendable);
		}

		public void Unregister(IEndlessModeBlendable blendable)
		{
			blendables.Remove(blendable);
		}
	}
}
