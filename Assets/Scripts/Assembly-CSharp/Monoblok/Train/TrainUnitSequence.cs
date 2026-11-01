using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZLinq;

namespace Monoblok.Train
{
	public class TrainUnitSequence
	{
		private readonly Train train;

		private List<TrainUnit> units = new List<TrainUnit>();

		private Bounds localBounds;

		public IReadOnlyList<TrainUnit> Units => units;

		public Bounds LocalBounds => localBounds;

		public int CartCount => units.AsValueEnumerable().Sum((TrainUnit u) => u.MemberCount);

		public IEnumerable<TrainCart> Carts => units.SelectMany((TrainUnit u) => u.GetCarts());

		public TrainUnitSequence(Train train)
		{
			this.train = train;
		}

		public void Add(TrainUnit unit, int index = -1)
		{
			foreach (TrainCart cart in unit.GetCarts())
			{
				if (Contains(cart))
				{
					Debug.LogError("Sequence already contains " + cart.gameObject.name);
					return;
				}
			}
			if (index == -1)
			{
				units.Add(unit);
			}
			else
			{
				units.Insert(index, unit);
			}
		}

		public void Remove(TrainUnit unit)
		{
			if (!units.Remove(unit))
			{
				Debug.LogError("Sequence doesn't contain unit");
			}
		}

		public void Remove(TrainCart cart)
		{
			if (!TryGetUnit(cart, out var unit))
			{
				Debug.LogError("No unit found with cart");
				return;
			}
			unit.RemoveCart(cart);
			if (unit.MemberCount == 0)
			{
				Remove(unit);
			}
		}

		public void Swap(TrainUnit a, TrainUnit b)
		{
			int num = units.IndexOf(a);
			int num2 = units.IndexOf(b);
			if (num == -1 || num2 == -1)
			{
				Debug.LogError("One or both units are not part of this sequence");
				return;
			}
			units[num] = b;
			units[num2] = a;
		}

		public int IndexOf(TrainUnit unit)
		{
			return units.IndexOf(unit);
		}

		public int UnitIndexOf(TrainCart cart)
		{
			if (!TryGetUnit(cart, out var unit))
			{
				Debug.LogError("No unit found with cart");
				return -1;
			}
			return IndexOf(unit);
		}

		private void CalculateBounds()
		{
			if (units.Count == 0)
			{
				localBounds = default(Bounds);
				return;
			}
			localBounds = units[0].LocalBounds;
			for (int i = 1; i < units.Count; i++)
			{
				localBounds.Encapsulate(units[i].LocalBounds);
			}
		}

		private bool Contains(TrainCart cart)
		{
			foreach (TrainUnit unit in units)
			{
				if (unit.Contains(cart))
				{
					return true;
				}
			}
			return false;
		}

		private bool TryGetUnit(TrainCart cart, out TrainUnit unit)
		{
			unit = units.AsValueEnumerable().FirstOrDefault((TrainUnit u) => u.Contains(cart));
			return unit != null;
		}

		public void Refresh(bool instant)
		{
			UpdateUnitPositions(instant);
			UpdateUnitStartingIndices();
			HandleParenting();
			CalculateBounds();
		}

		private void UpdateUnitPositions(bool instant)
		{
			_ = (Vector2)train.transform.position;
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < units.Count; i++)
			{
				TrainUnit trainUnit = units[i];
				trainUnit.SetLocalPosition(zero, instant);
				zero += trainUnit.LayoutSize;
			}
		}

		private void UpdateUnitStartingIndices()
		{
			int num = 0;
			for (int i = 0; i < units.Count; i++)
			{
				TrainUnit trainUnit = units[i];
				trainUnit.SetStartingIndex(num);
				num += trainUnit.MemberCount;
			}
		}

		private void HandleParenting()
		{
			List<TrainCart> list = (from c in units.AsValueEnumerable().SelectMany((TrainUnit u) => u.GetCarts())
				orderby c.Index
				select c).ToList();
			for (int i = 0; i < list.Count; i++)
			{
				TrainCart child = ((i < list.Count - 1) ? list[i + 1] : null);
				list[i].SetChild(child);
			}
		}
	}
}
