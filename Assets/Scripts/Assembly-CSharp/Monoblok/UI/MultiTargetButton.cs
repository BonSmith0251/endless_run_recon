using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.UI
{
	public class MultiTargetButton : Button
	{
		[SerializeField]
		private ButtonTarget[] targets;

		[SerializeField]
		private bool useUnscaledTime;

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			if (targets == null)
			{
				return;
			}
			ButtonTarget[] array = targets;
			foreach (ButtonTarget buttonTarget in array)
			{
				if (buttonTarget != null && !(buttonTarget.graphic == null))
				{
					Color color = GetColor(buttonTarget.colorBlock, state);
					if (!Application.isPlaying)
					{
						buttonTarget.graphic.color = color;
						continue;
					}
					Graphic graphic = buttonTarget.graphic;
					float fadeDuration = buttonTarget.colorBlock.fadeDuration;
					bool flag = useUnscaledTime;
					Tween.Color(graphic, color, fadeDuration, default(Easing), 1, CycleMode.Restart, 0f, 0f, flag);
				}
			}
		}

		private Color GetColor(ColorBlock block, SelectionState state)
		{
			return state switch
			{
				SelectionState.Normal => block.normalColor, 
				SelectionState.Highlighted => block.highlightedColor, 
				SelectionState.Pressed => block.pressedColor, 
				SelectionState.Selected => block.selectedColor, 
				SelectionState.Disabled => block.disabledColor, 
				_ => Color.white, 
			};
		}
	}
}
