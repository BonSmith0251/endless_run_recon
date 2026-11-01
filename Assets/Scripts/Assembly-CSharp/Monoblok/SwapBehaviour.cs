using System.Collections.Generic;
using Monoblok.Isometric;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	public class SwapBehaviour : IDraggable
	{
		private readonly TrainCart cart;

		private Vector3 offset;

		private int lastSwappedIndex;

		public SwapBehaviour(TrainCart cart)
		{
			this.cart = cart;
		}

		public void OnStartDrag(Vector3 worldPos)
		{
			if (cart.CartData.Orderable)
			{
				lastSwappedIndex = cart.Train.IndexOf(cart.Unit);
				offset = worldPos - cart.Unit.LocalPosition;
			}
		}

		public void OnDrag(Vector3 worldPos)
		{
			if (!cart.CartData.Orderable)
			{
				return;
			}
			int closestUnitIndex = GetClosestUnitIndex(worldPos);
			if (closestUnitIndex != lastSwappedIndex)
			{
				TrainUnit unitAtIndex = cart.Train.GetUnitAtIndex(closestUnitIndex);
				if (unitAtIndex != null && unitAtIndex != cart.Unit)
				{
					cart.Train.SwapUnits(cart.Unit, unitAtIndex);
					lastSwappedIndex = closestUnitIndex;
				}
			}
			cart.Unit.SetLocalPosition(worldPos - offset, instant: true);
		}

		public void OnEndDrag(Vector3 worldPos)
		{
			if (cart.CartData.Orderable)
			{
				cart.Train.Refresh();
				lastSwappedIndex = -1;
			}
		}

		private int GetClosestUnitIndex(Vector3 worldPos)
		{
			IReadOnlyList<TrainUnit> units = cart.Train.Units;
			int result = cart.Train.IndexOf(cart.Unit);
			float num = float.MaxValue;
			for (int i = 0; i < units.Count; i++)
			{
				if (units[i].Orderable)
				{
					float sqrMagnitude = Vector3.Project(worldPos - units[i].LocalBounds.center, (Vector3)Iso.Left).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = i;
					}
				}
			}
			return result;
		}
	}
}
