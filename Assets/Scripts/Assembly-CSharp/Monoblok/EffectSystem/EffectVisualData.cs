using UnityEngine;

namespace Monoblok.EffectSystem
{
	public abstract class EffectVisualData : ScriptableObject
	{
		public abstract EffectVisual GetVisual();
	}
}
