using Monoblok.Input;
using Monoblok.Train;
using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	public class RaycastCartSelectionStrategy : CartSelectionStrategy
	{
		private readonly Camera cam;

		private readonly LayerMask layerMask;

		private readonly InputReader inputReader;

		private TrainCart currentTarget;

		public RaycastCartSelectionStrategy(Camera cam, LayerMask layerMask, InputReader inputReader)
		{
			this.cam = cam;
			this.layerMask = layerMask;
			this.inputReader = inputReader;
		}

		protected override void OnInitialize()
		{
			SetCurrentTarget(null);
		}

		protected override void OnUpdate(float deltaTime)
		{
			FindTarget();
			HandleSubmit();
		}

		private void HandleSubmit()
		{
			if (inputReader.PointerReleasedThisFrame && !(currentTarget == null) && !(selectedCart.Value == currentTarget))
			{
				selectedCart.SetValue(currentTarget);
			}
		}

		private void FindTarget()
		{
			Vector2 pointerPosition = inputReader.PointerPosition;
			Vector3 vector = cam.ScreenToWorldPoint(pointerPosition);
			vector.z = -5f;
			RaycastHit2D[] array = Physics2D.RaycastAll(vector, Vector2.zero, 10f, layerMask);
			TrainCart trainCart = null;
			if (array.Length != 0)
			{
				float num = float.MaxValue;
				RaycastHit2D[] array2 = array;
				foreach (RaycastHit2D raycastHit2D in array2)
				{
					if (raycastHit2D.collider.TryGetComponent<TrainCart>(out var component))
					{
						float num2 = Vector2.Distance((Vector2)component.Center.position, (Vector2)vector);
						if (num2 < num)
						{
							num = num2;
							trainCart = component;
						}
					}
				}
			}
			SetCurrentTarget(trainCart);
		}

		private void SetCurrentTarget(TrainCart newTarget)
		{
			if (currentTarget != null)
			{
				Tween.Scale(currentTarget.transform, 1f, 0.15f);
			}
			currentTarget = newTarget;
			targetedCart.SetValue(currentTarget);
			if (!(currentTarget == null))
			{
				Tween.Scale(currentTarget.transform, 1.05f, 0.15f);
			}
		}

		protected override void OnDispose()
		{
			SetCurrentTarget(null);
		}
	}
}
