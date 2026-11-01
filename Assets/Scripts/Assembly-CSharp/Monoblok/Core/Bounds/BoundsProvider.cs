using UnityEngine;

namespace Monoblok.Core.Bounds
{
	public abstract class BoundsProvider : MonoBehaviour, IBoundsProvider
	{
		public abstract UnityEngine.Bounds GetBounds();
	}
}
