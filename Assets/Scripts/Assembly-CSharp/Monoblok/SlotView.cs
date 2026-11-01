using System;
using Monoblok.Core;
using Monoblok.Inventory;
using Monoblok.Train;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monoblok
{
	public class SlotView : Selectable
	{
		public enum State
		{
			Inactive,
			Free,
			Attached
		}

		[SerializeField]
		private GameObject outline;

		[SerializeField]
		private GameObject inactiveState;

		[SerializeField]
		private ModuleUIView moduleViewPrefab;

		[SerializeField]
		private RectTransform moduleContainer;

		private State state;

		public ModuleSlot Slot { get; private set; }

		public State CurrentState => state;

		public event Action<State> OnStateChanged;

		public event Action<SlotView> OnPointerEntered;

		public event Action<SlotView> OnPointerExited;

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (Slot != null)
			{
				Slot.OnModuleAdded -= OnModuleAdded;
				Slot.OnModuleRemoved -= OnModuleRemoved;
			}
		}

		public void Initialize(ModuleSlot slot)
		{
			if (Slot != null)
			{
				Slot.OnModuleAdded -= OnModuleAdded;
				Slot.OnModuleRemoved -= OnModuleRemoved;
			}
			if (slot == null)
			{
				ClearModule();
				ChangeState(State.Inactive);
				return;
			}
			Slot = slot;
			slot.OnModuleAdded += OnModuleAdded;
			slot.OnModuleRemoved += OnModuleRemoved;
			if (slot.ModuleData == null)
			{
				moduleContainer.Clear();
				ChangeState(State.Free);
			}
			else
			{
				OnModuleAdded(slot.ModuleData);
			}
		}

		private void OnModuleAdded(ModuleData data)
		{
			moduleContainer.Clear();
			UnityEngine.Object.Instantiate(moduleViewPrefab, moduleContainer).Initialize(data);
			ChangeState(State.Attached);
		}

		private void OnModuleRemoved()
		{
			ClearModule();
			ChangeState(State.Free);
		}

		private void ClearModule()
		{
			moduleContainer.Clear();
		}

		private void ChangeState(State newState)
		{
			switch (newState)
			{
			case State.Inactive:
				inactiveState.SetActive(value: true);
				base.interactable = false;
				break;
			case State.Free:
				outline.SetActive(value: false);
				inactiveState.SetActive(value: false);
				base.interactable = false;
				break;
			case State.Attached:
				outline.SetActive(value: false);
				inactiveState.SetActive(value: false);
				base.interactable = true;
				break;
			}
			state = newState;
			this.OnStateChanged?.Invoke(state);
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			outline.gameObject.SetActive(value: true);
			this.OnPointerEntered?.Invoke(this);
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			outline.gameObject.SetActive(value: false);
			this.OnPointerExited?.Invoke(this);
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			this.OnPointerEntered?.Invoke(this);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			this.OnPointerExited?.Invoke(this);
		}
	}
}
