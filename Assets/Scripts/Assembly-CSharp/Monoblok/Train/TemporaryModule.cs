using System;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Train
{
	public class TemporaryModule : ModuleBehavior
	{
		private TemporaryModuleDefinition.ExecutionTime executionTime;

		private Action<TrainCart> onComplete;

		public void Initialize(TemporaryModuleDefinition.ExecutionTime executionTime, Action<TrainCart> onComplete, Sprite sprite)
		{
			this.onComplete = onComplete;
			spriteRenderer.sprite = sprite;
			this.executionTime = executionTime;
		}

		protected override void OnModuleEnabled()
		{
			base.Slot.Cart.Train.RegisterWaitable(RemoveSeq);
			if (executionTime == TemporaryModuleDefinition.ExecutionTime.Instant)
			{
				onComplete?.Invoke(base.Slot.Cart);
			}
		}

		protected override void OnModuleDisabled()
		{
			if (base.Slot != null)
			{
				base.Slot.Cart.Train.RemoveWaitable(RemoveSeq);
				base.Slot.RemoveModule();
			}
		}

		private async UniTask RemoveSeq()
		{
			await Sequence.Create().Chain(Tween.PositionY(base.transform, base.transform.position.y + 1.5f, 0.1f, Ease.OutBack)).ChainDelay(0.1f)
				.Chain(Tween.Scale(base.transform, 0f, 0.1f, Ease.InBack));
			if (executionTime == TemporaryModuleDefinition.ExecutionTime.OnDelivered)
			{
				onComplete?.Invoke(base.Slot.Cart);
			}
			base.Slot.RemoveModule();
		}
	}
}
