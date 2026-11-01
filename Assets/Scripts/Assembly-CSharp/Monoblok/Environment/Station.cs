using Monoblok.Isometric;
using Monoblok.Progression;
using UnityEngine;

namespace Monoblok.Environment
{
	public class Station : EnvironmentMember
	{
		[SerializeField]
		private Sprite defaultSprite;

		[SerializeField]
		private Sprite lastStationSprite;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private Vector3 arrivalPosition;

		private Vector3 anchor;

		public Bounds Bounds => spriteRenderer.bounds;

		private void Awake()
		{
			arrivalPosition = base.transform.position;
			anchor = arrivalPosition;
		}

		private void OnEnable()
		{
			if (Application.isPlaying && !(progression == null))
			{
				progression.OnWaveEnded += OnWaveEnded;
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying && !(progression == null))
			{
				progression.OnWaveEnded -= OnWaveEnded;
			}
		}

		private void OnWaveEnded()
		{
			if (progression.CurrentWaveIndex == 19)
			{
				spriteRenderer.sprite = lastStationSprite;
			}
			else
			{
				spriteRenderer.sprite = defaultSprite;
			}
			anchor = arrivalPosition - (progression.GlobalDistance + progression.RampDownDistance) * (Vector3)Iso.Left;
		}

		public override void UpdateEnvironmentMember(float worldDistance, float normalizedSpeed)
		{
			base.transform.position = anchor + worldDistance * (Vector3)Iso.Left;
		}

		public override void ResetMember()
		{
			anchor = arrivalPosition;
			spriteRenderer.sprite = defaultSprite;
		}
	}
}
