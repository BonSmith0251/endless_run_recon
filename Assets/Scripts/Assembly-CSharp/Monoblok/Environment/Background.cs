using System.Linq;
using Monoblok.Progression;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Environment
{
	public class Background : EnvironmentMember
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private BackgroundColorThreshold[] thresholds;

		[SerializeField]
		private float mult;

		private MaterialPropertyBlock mpb;

		private static readonly int COLORCHANGE_ID = Shader.PropertyToID("_Color_Change");

		private static readonly int BASECOLOR_ID = Shader.PropertyToID("_BaseColor");

		private static readonly int NEXTCOLOR_ID = Shader.PropertyToID("_NextBaseColor");

		private static readonly int OFFSET_ID = Shader.PropertyToID("_OverlayOffset");

		private static readonly int WORLDTILING_ID = Shader.PropertyToID("_WorldTiling");

		private static readonly int OVERLAYTILING_ID = Shader.PropertyToID("_OverlayTiling");

		private Color _baseColor;

		private Color _nextColor;

		private int currentThreshold;

		private Tween colorChangeTween;

		private void Awake()
		{
			InitializeMPBIfNeeded();
		}

		private void InitializeMPBIfNeeded()
		{
			if (mpb == null)
			{
				mpb = new MaterialPropertyBlock();
			}
		}

		private void Start()
		{
			_baseColor = spriteRenderer.sharedMaterial.GetColor(BASECOLOR_ID);
		}

		private void OnEnable()
		{
			progression.OnWaveEnded += OnWaveEnded;
		}

		private void OnDisable()
		{
			progression.OnWaveEnded -= OnWaveEnded;
		}

		public override void Initialize(float maxSpeed, int waveCount)
		{
			base.Initialize(maxSpeed, waveCount);
			InitializeMPBIfNeeded();
			BackgroundColorThreshold backgroundColorThreshold = null;
			backgroundColorThreshold = ((waveCount != 0) ? (from t in thresholds
				where t.wave < waveCount
				orderby t.wave descending
				select t).FirstOrDefault() : thresholds.FirstOrDefault());
			if (backgroundColorThreshold != null)
			{
				spriteRenderer.GetPropertyBlock(mpb);
				mpb.SetColor(BASECOLOR_ID, backgroundColorThreshold.color);
				spriteRenderer.SetPropertyBlock(mpb);
				currentThreshold = backgroundColorThreshold.wave;
			}
		}

		private void OnWaveEnded()
		{
			if (thresholds == null)
			{
				return;
			}
			BackgroundColorThreshold[] array = thresholds;
			foreach (BackgroundColorThreshold backgroundColorThreshold in array)
			{
				if (backgroundColorThreshold.wave == progression.CurrentWaveIndex && currentThreshold != backgroundColorThreshold.wave)
				{
					_nextColor = backgroundColorThreshold.color;
					spriteRenderer.GetPropertyBlock(mpb);
					mpb.SetColor(NEXTCOLOR_ID, _nextColor);
					spriteRenderer.SetPropertyBlock(mpb);
					ChangeColor();
					currentThreshold = backgroundColorThreshold.wave;
					break;
				}
			}
		}

		private void ChangeColor()
		{
			colorChangeTween = Tween.Custom(this, 0f, 1f, 5f, delegate(Background t, float v)
			{
				t.SetColorChangeValue(v);
			}).OnComplete(this, delegate(Background t)
			{
				t.CleanupAfterColorChange();
			});
		}

		private void SetColorChangeValue(float value)
		{
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(COLORCHANGE_ID, value);
			spriteRenderer.SetPropertyBlock(mpb);
		}

		private void CleanupAfterColorChange()
		{
			_baseColor = _nextColor;
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetColor(BASECOLOR_ID, _baseColor);
			mpb.SetFloat(COLORCHANGE_ID, 0f);
			spriteRenderer.SetPropertyBlock(mpb);
		}

		public override void UpdateEnvironmentMember(float worldDistance, float normalizedSpeed)
		{
			Vector4 vector = spriteRenderer.material.GetVector(WORLDTILING_ID);
			Vector4 vector2 = spriteRenderer.material.GetVector(OVERLAYTILING_ID);
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(OFFSET_ID, worldDistance * vector.x * vector2.x);
			spriteRenderer.SetPropertyBlock(mpb);
		}

		public override void ResetMember()
		{
			colorChangeTween.Complete();
			colorChangeTween.Stop();
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetColor(BASECOLOR_ID, thresholds[0].color);
			spriteRenderer.SetPropertyBlock(mpb);
			currentThreshold = 0;
		}
	}
}
