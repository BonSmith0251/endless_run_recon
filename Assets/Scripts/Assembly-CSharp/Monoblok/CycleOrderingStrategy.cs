using Monoblok.Input;
using Monoblok.Train;

namespace Monoblok
{
	public class CycleOrderingStrategy : ICartOrderingStrategy
	{
		private readonly CartSelector cartSelector;

		private readonly InputReader inputReader;

		public CycleOrderingStrategy(CartSelector cartSelector, InputReader inputReader)
		{
			this.cartSelector = cartSelector;
			this.inputReader = inputReader;
		}

		public void Initialize()
		{
		}

		public void Dispose()
		{
		}

		public void Update(float deltaTime)
		{
			TrainCart selectedCart = cartSelector.SelectedCart;
			if (selectedCart == null)
			{
				return;
			}
			Monoblok.Train.Train train = selectedCart.Train;
			if (inputReader.SwapLeftPressed)
			{
				int index = train.UnitIndexOf(selectedCart) + 1;
				TrainUnit unitAtIndex = train.GetUnitAtIndex(index);
				if (unitAtIndex != null)
				{
					train.SwapUnits(selectedCart.Unit, unitAtIndex);
				}
			}
			else if (inputReader.SwapRightPressed)
			{
				int index2 = train.UnitIndexOf(selectedCart) - 1;
				TrainUnit unitAtIndex2 = train.GetUnitAtIndex(index2);
				if (unitAtIndex2 != null)
				{
					train.SwapUnits(selectedCart.Unit, unitAtIndex2);
				}
			}
		}
	}
}
