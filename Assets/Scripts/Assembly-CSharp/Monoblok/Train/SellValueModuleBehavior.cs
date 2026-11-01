using Cysharp.Threading.Tasks;
using Monoblok.Upgrades;
using UnityEngine;

namespace Monoblok.Train
{
	public class SellValueModuleBehavior : ModuleBehavior
	{
		private int step;

		public void Initialize(int step, Sprite sprite)
		{
			this.step = step;
			spriteRenderer.sprite = sprite;
		}

		protected override void OnModuleEnabled()
		{
			base.Slot.Cart.Train.RegisterWaitable(IncreaseWorth);
		}

		protected override void OnModuleDisabled()
		{
			base.Slot.Cart.Train.RemoveWaitable(IncreaseWorth);
		}

		private UniTask IncreaseWorth()
		{
			RuntimePrice worth = base.Slot.ModuleData.Worth;
			int value = Mathf.RoundToInt(worth.Amount.Value + step);
			worth.Amount.SetValue(value);
			return UniTask.CompletedTask;
		}
	}
}
