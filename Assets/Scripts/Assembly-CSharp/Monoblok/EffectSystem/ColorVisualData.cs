using UnityEngine;

namespace Monoblok.EffectSystem
{
	[CreateAssetMenu(menuName = "Game/Effect/Visual/Color Change")]
	public class ColorVisualData : EffectVisualData
	{
		[SerializeField]
		private Color targetColor;

		public override EffectVisual GetVisual()
		{
			return new ColorEffectVisual(targetColor);
		}
	}
}
