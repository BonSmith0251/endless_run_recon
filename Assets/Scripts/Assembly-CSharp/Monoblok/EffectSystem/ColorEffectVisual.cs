using UnityEngine;

namespace Monoblok.EffectSystem
{
	public class ColorEffectVisual : EffectVisual
	{
		private readonly Color targetColor;

		public ColorEffectVisual(Color targetColor)
		{
			this.targetColor = targetColor;
		}

		public override void Apply(EffectHandler target)
		{
			SpriteRenderer[] affectedRenderers = target.AffectedRenderers;
			foreach (SpriteRenderer spriteRenderer in affectedRenderers)
			{
				if (!(spriteRenderer == null))
				{
					spriteRenderer.color = targetColor;
				}
			}
		}

		public override void Remove(EffectHandler target)
		{
			SpriteRenderer[] affectedRenderers = target.AffectedRenderers;
			foreach (SpriteRenderer spriteRenderer in affectedRenderers)
			{
				if (!(spriteRenderer == null))
				{
					spriteRenderer.color = Color.white;
				}
			}
		}
	}
}
