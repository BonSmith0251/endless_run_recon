using Monoblok.Train;
using UnityEngine;

namespace Monoblok
{
	public class CartDraggable : MonoBehaviour, IDraggable
	{
		[SerializeField]
		private TrainCart cart;

		private SwapBehaviour swapBehaviour;

		private void Awake()
		{
			swapBehaviour = new SwapBehaviour(cart);
		}

		public void OnStartDrag(Vector3 worldPos)
		{
			swapBehaviour.OnStartDrag(worldPos);
		}

		public void OnDrag(Vector3 worldPos)
		{
			swapBehaviour.OnDrag(worldPos);
		}

		public void OnEndDrag(Vector3 worldPos)
		{
			swapBehaviour.OnEndDrag(worldPos);
		}
	}
}
