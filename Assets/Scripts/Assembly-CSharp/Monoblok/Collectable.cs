using UnityEngine;

namespace Monoblok
{
	public abstract class Collectable : MonoBehaviour
	{
		public abstract void Collect(Collector collector);
	}
}
