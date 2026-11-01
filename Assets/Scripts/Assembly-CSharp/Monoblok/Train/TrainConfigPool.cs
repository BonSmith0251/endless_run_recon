using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Train Config Pool")]
	public class TrainConfigPool : ScriptableObject
	{
		[SerializeField]
		private TrainConfigEntry[] entries;

		public IReadOnlyList<TrainConfigEntry> Entries => entries;

		public TrainConfig GetConfigById(string id)
		{
			TrainConfigEntry trainConfigEntry = entries.FirstOrDefault((TrainConfigEntry e) => e.Config.ID == id);
			if (trainConfigEntry == null)
			{
				Debug.LogError("No train config found in pool with id " + id);
			}
			return trainConfigEntry?.Config;
		}
	}
}
