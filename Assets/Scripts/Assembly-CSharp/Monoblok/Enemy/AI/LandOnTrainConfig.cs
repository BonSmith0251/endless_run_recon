using Monoblok.SA;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	[CreateAssetMenu(menuName = "Game/Enemy/AI/State/Land On Train")]
	public class LandOnTrainConfig : LandConfig
	{
		[SerializeField]
		private TrainRuntimeValue trainValue;

		public override Vector2 GetTarget()
		{
			return trainValue.Value.Bounds.center;
		}
	}
}
