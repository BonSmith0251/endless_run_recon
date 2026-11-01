using Monoblok.Input;
using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	public class MouseDragDropOrderingStrategy : ICartOrderingStrategy
	{
		private readonly InputReader inputReader;

		private readonly CartSelector cartSelector;

		private readonly Camera cam;

		private TrainCart dragging;

		public MouseDragDropOrderingStrategy(InputReader inputReader, CartSelector cartSelector, Camera cam)
		{
			this.inputReader = inputReader;
			this.cartSelector = cartSelector;
			this.cam = cam;
		}

		public void Initialize()
		{
		}

		public void Dispose()
		{
			dragging = null;
		}

		public void Update(float deltaTime)
		{
			Vector3 worldPos = cam.ScreenToWorldPoint(inputReader.PointerPosition);
			if (dragging != null)
			{
				dragging.Draggable.OnDrag(worldPos);
				if (inputReader.PointerReleasedThisFrame)
				{
					dragging.Draggable.OnEndDrag(worldPos);
					dragging = null;
				}
			}
			else
			{
				TrainCart targetedCart = cartSelector.TargetedCart;
				if (!(targetedCart == null) && inputReader.PointerPressedThisFrame)
				{
					targetedCart.Draggable.OnStartDrag(worldPos);
					dragging = targetedCart;
				}
			}
		}
	}
}
