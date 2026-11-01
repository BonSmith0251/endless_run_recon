using UnityEngine;

namespace Monoblok
{
	public abstract class EndlessModeBlendable : MonoBehaviour, IEndlessModeBlendable
	{
		[SerializeField]
		private EndlessModeTransition transitionManager;

		private void OnEnable()
		{
			transitionManager.Register(this);
		}

		private void OnDisable()
		{
			transitionManager.Unregister(this);
		}

		public abstract void Blend();

		public abstract void UnBlend();
	}
}
