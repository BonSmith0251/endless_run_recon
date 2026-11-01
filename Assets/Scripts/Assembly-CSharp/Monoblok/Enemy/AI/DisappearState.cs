using Monoblok.EffectSystem;
using PrimeTween;

namespace Monoblok.Enemy.AI
{
	public class DisappearState : EnemyState
	{
		public DisappearState(EnemyBrain brain, EnemyStateConfig config)
			: base(brain, config)
		{
		}

		protected override void OnEnter()
		{
			EffectHandler effectHandler = brain.EffectHandler;
			if (effectHandler != null)
			{
				effectHandler.ClearEffects();
			}
			Sequence.Create().Chain(Tween.Scale(brain.transform, 0f, 0.15f, Ease.InBack)).OnComplete(this, delegate(DisappearState t)
			{
				t.Finish();
			});
		}

		protected override void OnExit()
		{
		}

		protected override void OnUpdate(float deltaTime)
		{
		}
	}
}
