using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Monoblok.Timeline
{
	public class TypewriterMixerBehaviour : PlayableBehaviour
	{
		public double[] clipStartTimes;

		public double[] clipEndTimes;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			TMP_Text tMP_Text = playerData as TMP_Text;
			if (tMP_Text == null)
			{
				return;
			}
			double time = playable.GetTime();
			float num = 0f;
			for (int i = 0; i < clipStartTimes.Length; i++)
			{
				double num2 = clipStartTimes[i];
				double num3 = clipEndTimes[i];
				if (!(time < num2))
				{
					if (time >= num3)
					{
						num = 1f;
						continue;
					}
					double num4 = num3 - num2;
					num = ((num4 > 0.0) ? Mathf.Clamp01((float)((time - num2) / num4)) : 1f);
				}
			}
			int length = tMP_Text.text.Length;
			tMP_Text.maxVisibleCharacters = ((num >= 1f) ? length : Mathf.FloorToInt(num * (float)length));
		}
	}
}
