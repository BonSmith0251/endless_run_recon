using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Monoblok.Train
{
	public class TrainDamageOverlayController : MonoBehaviour
	{
		[SerializeField]
		private FullScreenPassRendererFeature feature;

		[SerializeField]
		private Train train;

		private static readonly int OpacityID = Shader.PropertyToID("_Opacity");

		private Material originalMaterial;

		private Material materialInstance;

		private void Awake()
		{
			originalMaterial = feature.passMaterial;
			materialInstance = Object.Instantiate(feature.passMaterial);
			materialInstance.SetFloat(OpacityID, 0f);
			feature.passMaterial = materialInstance;
		}

		private void OnDestroy()
		{
			if (materialInstance != null)
			{
				Object.Destroy(materialInstance);
			}
			if (originalMaterial != null)
			{
				feature.passMaterial = originalMaterial;
			}
		}

		private void OnEnable()
		{
			train.Health.OnHealthChanged += OnHealthChanged;
		}

		private void OnDisable()
		{
			train.Health.OnHealthChanged -= OnHealthChanged;
		}

		private void OnHealthChanged(int health)
		{
			float num = train.Health.MaxHealth;
			float endValue = ((num > 0f) ? (1f - (float)health / num) : 0f);
			Tween.MaterialProperty(materialInstance, OpacityID, endValue, 0.15f);
		}
	}
}
