using UnityEngine;

namespace Monoblok
{
	public interface IDraggable
	{
		void OnStartDrag(Vector3 worldPos);

		void OnDrag(Vector3 worldPos);

		void OnEndDrag(Vector3 worldPos);
	}
}
