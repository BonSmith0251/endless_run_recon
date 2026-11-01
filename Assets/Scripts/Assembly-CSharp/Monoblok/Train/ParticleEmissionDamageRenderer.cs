using UnityEngine;

namespace Monoblok.Train
{
	public class ParticleEmissionDamageRenderer : CartDamageRenderer
	{
		[SerializeField]
		private ParticleSystem ps;

		[SerializeField]
		private AnimationCurve emissionCurve;

		public override void OnHealthChanged(float normalizedHealth)
		{
			if (normalizedHealth < 1f && !ps.isPlaying)
			{
				ps.Play();
			}
			else if (normalizedHealth >= 1f && ps.isPlaying)
			{
				ps.Stop();
				return;
			}
			float num = emissionCurve.Evaluate(1f - normalizedHealth);
			ParticleSystem.EmissionModule emission = ps.emission;
			emission.rateOverTime = num;
		}
	}
}
