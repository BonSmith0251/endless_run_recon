using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Train
{
	[RequireComponent(typeof(StatManager))]
	public abstract class ModuleBehavior : MonoBehaviour
	{
		[SerializeField]
		protected StatManager statManager;

		[SerializeField]
		protected SpriteRenderer spriteRenderer;

		private ModuleSlot slot;

		private bool moduleEnabled;

		public StatManager StatManager => statManager;

		public ModuleSlot Slot => slot;

		public bool IsAttached => slot != null;

		public bool ModuleEnabled => moduleEnabled;

		public SpriteRenderer SpriteRenderer => spriteRenderer;

		private void OnValidate()
		{
			if (statManager == null)
			{
				statManager = GetComponent<StatManager>();
			}
		}

		public void Attach(ModuleSlot slot)
		{
			if (slot != null && slot != this.slot)
			{
				this.slot = slot;
				base.transform.localPosition = slot.LocalPosition;
				UpdateSortingOrder();
				OnAttached();
				SetEnabled(enabled: true);
			}
		}

		public void Detach()
		{
			if (slot != null)
			{
				SetEnabled(enabled: false);
				OnDetached();
				slot = null;
			}
		}

		public void UpdateSortingOrder()
		{
			spriteRenderer.sortingLayerName = slot.Cart.SpriteRenderer.sortingLayerName;
			spriteRenderer.sortingOrder = slot.Cart.SpriteRenderer.sortingOrder + 1 + slot.Index;
		}

		public void SetEnabled(bool enabled)
		{
			if (enabled != moduleEnabled)
			{
				moduleEnabled = enabled;
				if (moduleEnabled)
				{
					OnModuleEnabled();
				}
				else
				{
					OnModuleDisabled();
				}
			}
		}

		public virtual void OnCartSelected()
		{
		}

		public virtual void OnCartDeselected()
		{
		}

		public virtual void OnCartTargetStarted()
		{
		}

		public virtual void OnCartTargetEnded()
		{
		}

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetached()
		{
		}

		protected virtual void OnModuleEnabled()
		{
		}

		protected virtual void OnModuleDisabled()
		{
		}
	}
}
