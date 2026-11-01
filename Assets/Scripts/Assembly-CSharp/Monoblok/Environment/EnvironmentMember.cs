using UnityEngine;

namespace Monoblok.Environment
{
	public abstract class EnvironmentMember : MonoBehaviour, IEnvironmentMember
	{
		protected float maxSpeed;

		public virtual void Initialize(float maxSpeed, int waveCount)
		{
			this.maxSpeed = maxSpeed;
		}

		public abstract void UpdateEnvironmentMember(float globalDistance, float normalizedSpeed);

		public virtual void ResetMember()
		{
		}
	}
}
