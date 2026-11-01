using UnityEngine;

namespace Monoblok.EffectSystem
{
	public class ParticleEffectVisual : EffectVisual
	{
		private readonly ParticleSystem psPrefab;

		private ParticleSystem instance;

		public ParticleEffectVisual(ParticleSystem psPrefab)
		{
			this.psPrefab = psPrefab;
		}

		public override void Apply(EffectHandler target)
		{
			instance = Object.Instantiate(psPrefab, target.transform);
		}

		public override void Remove(EffectHandler target)
		{
			Object.Destroy(instance.gameObject);
		}
	}
}
