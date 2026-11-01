using System;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Train
{
	[Serializable]
	public class TrainConfigEntry
	{
		[field: SerializeField]
		public TrainConfig Config { get; private set; }

		[field: SerializeField]
		public BoolRuntimeValue UnlockFlag { get; private set; }

		public bool IsUnlocked
		{
			get
			{
				if (!(UnlockFlag == null))
				{
					return UnlockFlag.Value;
				}
				return true;
			}
		}
	}
}
