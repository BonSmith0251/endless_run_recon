using System;
using System.Linq;
using FMODUnity;
using Monoblok.Input;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	public class CartSelector : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Train.Train train;

		[SerializeField]
		private LayerMask layerMask;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private SelectorRectangle reticle;

		[SerializeField]
		private EventReference selectRef;

		private int startingIndex;

		private ICartSelectionStrategy selectionStrategy;

		private bool selectionEnabled;

		private TrainCart currentTarget;

		private TrainCart currentSelection;

		public TrainCart SelectedCart => selectionStrategy?.SelectedCart.Value;

		public TrainCart TargetedCart => selectionStrategy?.TargetedCart.Value;

		public event Action<TrainCart> OnSelectionChanged;

		private void OnEnable()
		{
			InputManager.OnInputDeviceChanged += HandleInputDeviceChanged;
			HandleInputDeviceChanged(InputManager.CurrentDevice);
		}

		private void OnDisable()
		{
			InputManager.OnInputDeviceChanged -= HandleInputDeviceChanged;
		}

		private void Update()
		{
			if (selectionEnabled)
			{
				selectionStrategy?.Update(Time.deltaTime);
			}
		}

		private void HandleInputDeviceChanged(InputManager.InputDeviceType type)
		{
			SetStrategy(type switch
			{
				InputManager.InputDeviceType.KBM => new RaycastCartSelectionStrategy(Camera.main, layerMask, inputReader), 
				InputManager.InputDeviceType.Gamepad => new CycleCartSelectionStrategy(train, inputReader), 
				_ => null, 
			});
		}

		private void SetStrategy(ICartSelectionStrategy newStrategy)
		{
			TrainCart initialSelection = selectionStrategy?.SelectedCart.Value ?? train.Carts.ElementAt(startingIndex);
			TeardownStrategy();
			selectionStrategy = newStrategy;
			if (selectionEnabled)
			{
				ActivateStrategy(initialSelection);
			}
		}

		private void ActivateStrategy(TrainCart initialSelection)
		{
			if (selectionStrategy != null)
			{
				Debug.Log("Activating strategy");
				selectionStrategy.SelectedCart.OnValueChanged += HandleSelectionChanged;
				selectionStrategy.TargetedCart.OnValueChanged += HandleTargetChanged;
				selectionStrategy.Initialize(initialSelection);
				HandleSelectionChanged(selectionStrategy.SelectedCart.Value);
			}
		}

		private void TeardownStrategy()
		{
			if (selectionStrategy != null)
			{
				selectionStrategy.SelectedCart.OnValueChanged -= HandleSelectionChanged;
				selectionStrategy.TargetedCart.OnValueChanged -= HandleTargetChanged;
				selectionStrategy.Dispose();
				if (currentSelection != null)
				{
					currentSelection.OnDeselect();
				}
				if (currentTarget != null)
				{
					currentTarget.OnStopTarget();
				}
				currentSelection = null;
				currentTarget = null;
				this.OnSelectionChanged?.Invoke(null);
			}
		}

		private void HandleSelectionChanged(TrainCart cart)
		{
			RuntimeManager.PlayOneShot(selectRef);
			reticle.Show(cart.SpriteRenderer);
			if (currentSelection != null)
			{
				currentSelection.OnDeselect();
			}
			cart.OnSelect();
			this.OnSelectionChanged?.Invoke(cart);
		}

		private void HandleTargetChanged(TrainCart cart)
		{
			if (currentTarget != null)
			{
				currentTarget.OnStopTarget();
			}
			currentTarget = cart;
			if (!(cart == null))
			{
				cart.OnStartTarget();
			}
		}

		public void ForceSelection(TrainCart cart)
		{
			if (!(cart == null) && selectionStrategy != null)
			{
				TeardownStrategy();
				ActivateStrategy(cart);
			}
		}

		public void EnableSelection()
		{
			Debug.Log("Enable selection");
			selectionEnabled = true;
			ActivateStrategy(train.Carts.ElementAt(startingIndex));
		}

		public void DisableSelection()
		{
			selectionEnabled = false;
			reticle.Hide();
			TeardownStrategy();
		}
	}
}
