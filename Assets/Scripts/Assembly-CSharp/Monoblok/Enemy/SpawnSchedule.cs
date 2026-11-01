using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.Enemy
{
	[CreateAssetMenu(menuName = "Game/Enemy/Spawn Schedule")]
	public class SpawnSchedule : ScriptableObject
	{
		[SerializeField]
		private SpawnScheduleEntry[] entries;

		[SerializeField]
		private ExactSpawnEntry[] exactEntries;

		[SerializeField]
		private RecurringSpawnEntry[] recurringEntries;

		private float[] _weights;

		private void OnEnable()
		{
			_weights = new float[entries.Length];
		}

		public EnemyGroup Get(int patternWave)
		{
			float num = 0f;
			for (int i = 0; i < entries.Length; i++)
			{
				_weights[i] = Mathf.Max(0f, entries[i].weightCurve.Evaluate(patternWave));
				num += _weights[i];
			}
			if (num <= 0f)
			{
				return null;
			}
			float num2 = Random.Range(0f, num);
			float num3 = 0f;
			for (int j = 0; j < entries.Length; j++)
			{
				num3 += _weights[j];
				if (num2 <= num3)
				{
					return entries[j].group.Get();
				}
			}
			return entries[entries.Length - 1].group.Get();
		}

		public IEnumerable<EnemyGroup> GetExactGroups(int waveIndex, int patternWave)
		{
			ExactSpawnEntry[] array = exactEntries;
			foreach (ExactSpawnEntry exactSpawnEntry in array)
			{
				if (exactSpawnEntry.wave == patternWave)
				{
					yield return exactSpawnEntry.group.Get();
				}
			}
			RecurringSpawnEntry[] array2 = recurringEntries;
			foreach (RecurringSpawnEntry recurringSpawnEntry in array2)
			{
				if (recurringSpawnEntry.interval > 0 && waveIndex > 0 && waveIndex % recurringSpawnEntry.interval == 0)
				{
					yield return recurringSpawnEntry.group.Get();
				}
			}
		}
	}
}
