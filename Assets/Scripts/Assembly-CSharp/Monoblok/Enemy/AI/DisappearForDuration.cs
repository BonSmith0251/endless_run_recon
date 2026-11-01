using Monoblok.EffectSystem;
using PrimeTween;

namespace Monoblok.Enemy.AI
{
	public class DisappearForDuration : EnemyState
	{
		private DisappearForDurationStateConfig config;

		public DisappearForDuration(EnemyBrain brain, DisappearForDurationStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			EffectHandler effectHandler = brain.EffectHandler;
			if (effectHandler != null)
			{
				effectHandler.ClearEffects();
			}
			brain.BoxCollider.enabled = false;
			Sequence.Create().Chain(Tween.Scale(brain.transform, 0f, 0.15f, Ease.InBack)).ChainDelay(config.Duration)
				.Chain(Tween.Scale(brain.transform, 1f, 0.15f, Ease.OutBack))
				.OnComplete(this, delegate(DisappearForDuration t)
				{
					t.CompleteDisappear();
				});
		}

		private void CompleteDisappear()
		{
			brain.BoxCollider.enabled = true;
			Finish();
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
