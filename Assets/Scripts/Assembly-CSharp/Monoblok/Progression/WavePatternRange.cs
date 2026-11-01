using System;
using UnityEngine;

namespace Monoblok.Progression
{
	[Serializable]
	public class WavePatternRange
	{
		public Vector2Int waveRange;

		[SerializeReference]
		[SubclassSelector]
		public IWaveFactory waveFactory;
	}
}
