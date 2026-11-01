using System.Collections.Generic;
using Monoblok.Isometric;
using PrimeTween;
using UnityEngine;
using ZLinq;

namespace Monoblok.Train
{
	public class TrainUnit
	{
		private class TrainUnitMember
		{
			public readonly TrainUnit Unit;

			public readonly TrainCart Cart;

			public Vector3 localPosition;

			public Vector3 WorldPosition => Unit.WorldPosition + localPosition;

			public TrainUnitMember(TrainUnit unit, TrainCart cart)
			{
				Unit = unit;
				Cart = cart;
			}

			public void UpdatePosition(bool instant = false)
			{
				Vector3 vector = Unit.WorldPosition + localPosition;
				if (instant)
				{
					Cart.transform.position = vector;
				}
				else
				{
					Tween.Position(Cart.transform, vector, 0.1f);
				}
			}
		}

		private readonly float cartGap;

		private readonly Train train;

		private bool orderable = true;

		private int startingIndex;

		private List<TrainUnitMember> members = new List<TrainUnitMember>();

		private Vector3 localPosition = Vector3.zero;

		private Bounds localBounds;

		private Vector2 layoutSize;

		public CartMergeConnection mergeConnection;

		public Vector2 LayoutSize => layoutSize;

		public int StartingIndex => startingIndex;

		public bool Orderable => orderable;

		public Vector3 LocalPosition => localPosition;

		public Vector3 WorldPosition => train.transform.position + localPosition;

		public int MemberCount => members.Count;

		public Bounds LocalBounds => localBounds;

		public TrainUnit(Train train, float cartGap = 0f, params TrainCart[] carts)
		{
			this.train = train;
			this.cartGap = cartGap;
			foreach (TrainCart cart in carts)
			{
				AddCart(cart);
			}
		}

		public static TrainUnit Merge(TrainUnit a, TrainUnit b)
		{
			if (a == null || b == null)
			{
				Debug.LogError("Trying to merge, but either one or both units are null");
				return null;
			}
			TrainCart[] carts = a.GetCarts().AsValueEnumerable().Concat(b.GetCarts())
				.ToArray();
			return new TrainUnit(a.train, a.cartGap, carts);
		}

		public void SetLocalPosition(Vector3 localPosition, bool instant = false)
		{
			this.localPosition = localPosition;
			UpdateMemberPositions(instant);
			CalculateBounds();
		}

		public void SetStartingIndex(int index)
		{
			startingIndex = index;
			UpdateMemberIndices();
			UpdateSortingOrder();
		}

		public void AddCart(TrainCart cart)
		{
			if (TryGetMember(cart, out var _))
			{
				Debug.LogError(cart.gameObject.name + " is already a part of this unit");
				return;
			}
			cart.SetUnit(this);
			members.Add(new TrainUnitMember(this, cart));
			if (!cart.CartData.Orderable)
			{
				orderable = false;
			}
			Refresh();
		}

		public void RemoveCart(TrainCart cart)
		{
			if (!TryGetMember(cart, out var member))
			{
				Debug.LogError(cart.gameObject.name + " is not part of this unit");
				return;
			}
			members.Remove(member);
			Refresh();
		}

		public IEnumerable<TrainCart> GetCarts()
		{
			foreach (TrainUnitMember member in members)
			{
				if (member != null && !(member.Cart == null))
				{
					yield return member.Cart;
				}
			}
		}

		public bool Contains(TrainCart cart)
		{
			foreach (TrainCart cart2 in GetCarts())
			{
				if (cart == cart2)
				{
					return true;
				}
			}
			return false;
		}

		private bool TryGetMember(TrainCart cart, out TrainUnitMember member)
		{
			member = null;
			foreach (TrainUnitMember member2 in members)
			{
				if (!(member2.Cart == null) && !(member2.Cart != cart))
				{
					member = member2;
					return true;
				}
			}
			return false;
		}

		private void Refresh()
		{
			CalculateMemberPositions();
			CalculateBounds();
		}

		private void UpdateMemberIndices()
		{
			for (int i = 0; i < members.Count; i++)
			{
				TrainUnitMember trainUnitMember = members[i];
				if (!(trainUnitMember.Cart == null))
				{
					trainUnitMember.Cart.Index = startingIndex + i;
				}
			}
		}

		private void CalculateMemberPositions()
		{
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < members.Count; i++)
			{
				TrainUnitMember trainUnitMember = members[i];
				TrainCart cart = trainUnitMember.Cart;
				if (!(cart == null))
				{
					trainUnitMember.localPosition = zero;
					_ = cart.SpriteRenderer.sprite.bounds.size;
					zero += Iso.Left * cart.CartData.FootPrint + Iso.Left * cartGap;
				}
			}
			layoutSize = zero;
		}

		private void UpdateMemberPositions(bool instant = false)
		{
			foreach (TrainUnitMember member in members)
			{
				member.UpdatePosition(instant);
			}
		}

		private void UpdateSortingOrder()
		{
			for (int i = 0; i < members.Count; i++)
			{
				int order = -(startingIndex + i) * 10;
				members[i].Cart.UpdateSortingOrder(order);
			}
		}

		private void CalculateBounds()
		{
			if (members.Count == 0)
			{
				localBounds = new Bounds(localPosition, Vector3.zero);
				return;
			}
			List<TrainUnitMember> list = members;
			TrainUnitMember trainUnitMember = list[list.Count - 1];
			Vector3 size = trainUnitMember.Cart.SpriteRenderer.sprite.bounds.size;
			Vector3 vector = new Vector3(trainUnitMember.localPosition.x - size.x, trainUnitMember.localPosition.y + size.y);
			Vector3 zero = Vector3.zero;
			Vector3 vector2 = (vector + zero) / 2f;
			localBounds = new Bounds(size: new Vector3(Mathf.Abs(zero.x - vector.x), Mathf.Abs(zero.y - vector.y)), center: localPosition + vector2);
		}
	}
}
