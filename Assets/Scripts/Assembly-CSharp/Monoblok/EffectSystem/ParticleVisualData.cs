using UnityEngine;

namespace Monoblok.EffectSystem
{
	[CreateAssetMenu(menuName = "Game/Effect/Visual/Particle System")]
	public class ParticleVisualData : EffectVisualData
	{
		[SerializeField]
		private ParticleSystem psPrefab;

		public override EffectVisual GetVisual()
		{
			return new ParticleEffectVisual(psPrefab);
		}
	}
}
