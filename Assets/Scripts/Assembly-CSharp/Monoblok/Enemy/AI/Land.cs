using PrimeTween;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class Land : EnemyState
	{
		private readonly float delay;

		private Vector2 target;

		private GameObject signal;

		public Land(EnemyBrain brain, LandConfig config)
			: base(brain, config)
		{
			delay = config.Delay;
			target = config.GetTarget();
		}

		protected override void OnEnter()
		{
			signal = Object.Instantiate(brain.SignalPrefab, null);
			signal.transform.position = target;
			signal.SetActive(value: true);
			brain.transform.position = target + Vector2.up * 40f;
			Sequence.Create().ChainDelay(delay).Chain(Tween.Position(brain.transform, target, 0.5f, Ease.InExpo))
				.ChainCallback(delegate
				{
					CameraShaker.Shake(ShakePreset.Heavy);
				})
				.OnComplete(this, delegate(Land t)
				{
					t.Finish();
				});
		}

		protected override void OnExit()
		{
			Object.Destroy(signal);
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
