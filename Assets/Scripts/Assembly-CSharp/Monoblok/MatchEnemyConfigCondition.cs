using System;
using Monoblok.Enemy;
using UnityEngine;

namespace Monoblok
{
	[Serializable]
	public class MatchEnemyConfigCondition : IGenericGameEventSinkCondition<Monoblok.Enemy.Enemy>
	{
		[SerializeField]
		private EnemyConfig config;

		public bool Evaluate(Monoblok.Enemy.Enemy data)
		{
			return data.Config == config;
		}
	}
}
