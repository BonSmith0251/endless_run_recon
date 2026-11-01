using JetBrains.Annotations;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace Monoblok
{
	public static class PrimeTweenExitensions
	{
		public static Tween TypewriterAnimation([NotNull] TMP_Text textComponent, string text, float charsPerSecond)
		{
			textComponent.text = text;
			int length = text.Length;
			float duration = (float)length / charsPerSecond;
			return Tween.TextMaxVisibleCharacters(textComponent, 0, length, duration, Ease.Linear);
		}

		public static Tween Counter([NotNull] TMP_Text text, int startValue, int endValue, float duration, Ease ease = Ease.Default, bool useUnscaledTime = false)
		{
			return Tween.Custom(text, startValue, endValue, duration, delegate(TMP_Text target, float val)
			{
				target.SetText("{0:0}", val);
			}, ease, 1, CycleMode.Restart, 0f, 0f, useUnscaledTime);
		}

		public static Sequence Jump([NotNull] Transform target, Vector3 endValue, float duration, float height)
		{
			Vector3 position = target.position;
			Vector3 endValue2 = new Vector3((position.x + endValue.x) / 2f, Mathf.Max(position.y, endValue.y) + height, endValue.z);
			float duration2 = duration / 2f;
			return Sequence.Create().Chain(Tween.Position(target, endValue2, duration2, Ease.OutQuad)).Chain(Tween.Position(target, endValue, duration2, Ease.InQuad));
		}
	}
}
