using Monoblok.Progression;
using UnityEngine;

namespace Monoblok.Train
{
	public class TrainDust : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem ps;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private float rateOverTimeMultiplier = 1f;

		private void Update()
		{
			if (!(ps == null))
			{
				ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = ps.velocityOverLifetime;
				ParticleSystem.MinMaxCurve y = velocityOverLifetime.y;
				y.constantMax = progression.Speed;
				y.constantMin = Mathf.Max(progression.Speed - 3f, 0f);
				velocityOverLifetime.y = y;
				ParticleSystem.EmissionModule emission = ps.emission;
				emission.rateOverTime = progression.Speed * rateOverTimeMultiplier;
			}
		}
	}
}
