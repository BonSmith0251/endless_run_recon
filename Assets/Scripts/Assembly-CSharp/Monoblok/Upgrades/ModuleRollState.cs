using System.Collections.Generic;
using Monoblok.Train;

namespace Monoblok.Upgrades
{
	public class ModuleRollState
	{
		private readonly Dictionary<ModuleDefinition, float> scores = new Dictionary<ModuleDefinition, float>();

		private const float minScore = 0f;

		private const float baseScore = 1f;

		private const float recoveryPerRoll = 0.5f;

		public float GetWeight(ModuleDefinition module)
		{
			if (!scores.TryGetValue(module, out var value))
			{
				value = 1f;
				scores[module] = value;
			}
			return value;
		}

		public void OnOffered(ModuleDefinition module)
		{
			scores[module] = 0f;
		}

		public void OnNotOffered(ModuleDefinition module)
		{
			scores[module] = GetWeight(module) + 0.5f;
		}
	}
}
