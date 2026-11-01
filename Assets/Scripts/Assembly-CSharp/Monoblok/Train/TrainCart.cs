using System;
using System.Collections.Generic;
using Monoblok.Core;
using Monoblok.DamageSystem;
using Monoblok.StatSystem;
using Monoblok.Weapon;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Train
{
	[SelectionBase]
	public class TrainCart : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private TrainCartDamageable damageable;

		[SerializeField]
		private PolygonCollider2D hitboxCollider;

		[SerializeField]
		private ParticleSystem explosionParticles;

		[SerializeField]
		private BoxCollider2D boxCollider;

		[SerializeField]
		private Transform center;

		[SerializeField]
		private StatManager statManager;

		[SerializeField]
		private CartDraggable draggable;

		[SerializeField]
		private ModuleManager moduleManager;

		[SerializeField]
		private SpriteHighlighter spriteHighlighter;

		private Train train;

		private TrainUnit unit;

		private CartData data;

		private CartConnectionSlot parentSlot;

		private CartConnectionSlot childSlot;

		private readonly List<WeaponHandler> weaponHandlers = new List<WeaponHandler>();

		[ShowNativeProperty]
		public int Index { get; set; }

		[ShowNativeProperty]
		public int UnitIndex => train.UnitIndexOf(this);

		public Train Train => train;

		public TrainUnit Unit => unit;

		public CartData CartData => data;

		public SpriteRenderer SpriteRenderer => spriteRenderer;

		public TrainCartDamageable Damageable => damageable;

		public Transform Center => center;

		public CartDraggable Draggable => draggable;

		public StatManager StatManager => statManager;

		public ModuleManager ModuleManager => moduleManager;

		public ParticleSystem ExplosionParticles => explosionParticles;

		public IReadOnlyList<WeaponHandler> WeaponHandlers => weaponHandlers;

		[ShowNativeProperty]
		public TrainCart Parent
		{
			get
			{
				if (parentSlot != null)
				{
					return parentSlot.Connection;
				}
				return null;
			}
		}

		[ShowNativeProperty]
		public TrainCart Child
		{
			get
			{
				if (childSlot != null)
				{
					return childSlot.Connection;
				}
				return null;
			}
		}

		public event Action<TrainCart> OnChildAdded;

		public event Action<TrainCart> OnParented;

		public event Action<WeaponHandler> OnWeaponHandlerRegistered;

		public event Action<WeaponHandler> OnWeaponHandlerUnregistered;

		private void OnValidate()
		{
			if (statManager == null)
			{
				statManager = GetComponentInChildren<StatManager>();
			}
			if (moduleManager == null)
			{
				moduleManager = GetComponentInChildren<ModuleManager>();
			}
			if (hitboxCollider == null)
			{
				hitboxCollider = GetComponentInChildren<PolygonCollider2D>();
			}
			if (boxCollider == null)
			{
				boxCollider = GetComponent<BoxCollider2D>();
			}
		}

		private void Awake()
		{
			center.transform.position = spriteRenderer.bounds.center;
			parentSlot = new CartConnectionSlot();
			childSlot = new CartConnectionSlot();
		}

		public void SetUnit(TrainUnit unit)
		{
			this.unit = unit;
		}

		public void UpdateSortingOrder(int order)
		{
			spriteRenderer.sortingOrder = order;
			moduleManager.UpdateSortingOrder();
		}

		public void OnSelect()
		{
			foreach (ModuleSlot slot in moduleManager.Slots)
			{
				if (!slot.IsFree)
				{
					slot.Behavior.OnCartSelected();
				}
			}
		}

		public void OnDeselect()
		{
			foreach (ModuleSlot slot in moduleManager.Slots)
			{
				if (!slot.IsFree)
				{
					slot.Behavior.OnCartDeselected();
				}
			}
		}

		public void OnStartTarget()
		{
			foreach (ModuleSlot slot in moduleManager.Slots)
			{
				if (!slot.IsFree)
				{
					slot.Behavior.OnCartTargetStarted();
				}
			}
		}

		public void OnStopTarget()
		{
			foreach (ModuleSlot slot in moduleManager.Slots)
			{
				if (!slot.IsFree)
				{
					slot.Behavior.OnCartTargetEnded();
				}
			}
		}

		public void EnableHitboxCollider(bool enabled)
		{
			if (!(hitboxCollider == null) && hitboxCollider.enabled != enabled)
			{
				hitboxCollider.enabled = enabled;
			}
		}

		public void Initialize(Train train, CartData cartData)
		{
			this.train = train;
			data = cartData;
			Sprite cartSprite = cartData.CartSprite;
			spriteRenderer.sprite = cartSprite;
			center.transform.localPosition = cartSprite.bounds.center;
			boxCollider.offset = cartSprite.bounds.center;
			boxCollider.size = cartSprite.bounds.size;
			hitboxCollider.CreateFromSprite(cartSprite, 0.25f, 200);
			moduleManager.Initialize(cartData.SlotLocalPositions, cartData.ShowModuleVisuals);
		}

		public void SetChild(TrainCart child)
		{
			childSlot.Disconnect();
			if (!(child == null))
			{
				childSlot.Connect(child);
				child.parentSlot.Connect(this);
				child.OnParented?.Invoke(this);
				this.OnChildAdded?.Invoke(child);
			}
		}

		public void ClearConnections()
		{
			parentSlot.Disconnect();
			childSlot.Disconnect();
		}

		public void RegisterWeaponHandler(WeaponHandler handler)
		{
			weaponHandlers.Add(handler);
			this.OnWeaponHandlerRegistered?.Invoke(handler);
		}

		public void UnregisterWeaponHandler(WeaponHandler handler)
		{
			if (weaponHandlers.Remove(handler))
			{
				this.OnWeaponHandlerUnregistered?.Invoke(handler);
			}
		}
	}
}
