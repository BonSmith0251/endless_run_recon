using Monoblok.Input;
using Monoblok.Train;

namespace Monoblok
{
	public class CycleCartSelectionStrategy : CartSelectionStrategy
	{
		private readonly Monoblok.Train.Train train;

		private readonly InputReader inputReader;

		public CycleCartSelectionStrategy(Monoblok.Train.Train train, InputReader inputReader)
		{
			this.train = train;
			this.inputReader = inputReader;
		}

		protected override void OnUpdate(float deltaTime)
		{
			TrainCart next2;
			if (inputReader.CycleLeftPressed)
			{
				if (TryGetCartByOffset(1, out var next))
				{
					selectedCart.SetValue(next);
					targetedCart.SetValue(next);
				}
			}
			else if (inputReader.CycleRightPressed && TryGetCartByOffset(-1, out next2))
			{
				selectedCart.SetValue(next2);
				targetedCart.SetValue(next2);
			}
		}

		private bool TryGetCartByOffset(int offset, out TrainCart next)
		{
			next = null;
			TrainCart value = selectedCart.Value;
			if (value == null)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			foreach (TrainCart cart in train.Carts)
			{
				if (cart.Index == value.Index)
				{
					num = num2;
					break;
				}
				num2++;
			}
			int num3 = (num + offset + train.CartCount) % train.CartCount;
			num2 = 0;
			foreach (TrainCart cart2 in train.Carts)
			{
				if (num2 == num3)
				{
					next = cart2;
					return true;
				}
				num2++;
			}
			return false;
		}
	}
}
