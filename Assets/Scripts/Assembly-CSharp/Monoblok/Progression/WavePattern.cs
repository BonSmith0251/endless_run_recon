using System.Linq;
using UnityEngine;

namespace Monoblok.Progression
{
	[CreateAssetMenu(menuName = "Game/Progression/Wave Pattern")]
	public class WavePattern : ScriptableObject
	{
		[SerializeField]
		private WavePatternRange[] ranges;

		public Wave GetWave(int waveIndex)
		{
			int num = ranges.Select((WavePatternRange r) => r.waveRange.x).Min();
			int modulus = ranges.Select((WavePatternRange r) => r.waveRange.y).Max() - num + 1;
			int patternPosition = num + Mod(waveIndex, modulus);
			WavePatternRange wavePatternRange = ranges.FirstOrDefault((WavePatternRange r) => patternPosition >= r.waveRange.x && patternPosition <= r.waveRange.y);
			if (wavePatternRange == null)
			{
				Debug.LogWarning($"[WavePattern] No range found for wave {waveIndex} (pattern position {patternPosition}).");
				return null;
			}
			return wavePatternRange.waveFactory.GetWave(waveIndex);
		}

		private static int Mod(int value, int modulus)
		{
			return (value % modulus + modulus) % modulus;
		}
	}
}
