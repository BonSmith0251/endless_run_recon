using UnityEngine;

namespace Monoblok.Train
{
	public abstract class CartDamageRenderer : MonoBehaviour
	{
		public abstract void OnHealthChanged(float normalizedHealth);
	}
}
