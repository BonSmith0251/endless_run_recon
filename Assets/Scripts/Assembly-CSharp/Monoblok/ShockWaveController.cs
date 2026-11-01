using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	public class ShockWaveController : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private Camera cam;

		[SerializeField]
		private ParticleSystem ps;

		private MaterialPropertyBlock mpb;

		private Tween waveTween;

		private static readonly int CenterID = Shader.PropertyToID("_Center");

		private static readonly int WaveDistanceID = Shader.PropertyToID("_WaveDistanceFromCenter");

		private static readonly int RadiusID = Shader.PropertyToID("_Radius");

		public static ShockWaveController Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			mpb = new MaterialPropertyBlock();
			if (cam == null)
			{
				cam = Camera.main;
			}
			spriteRenderer.enabled = false;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public void TriggerShockWave(Vector3 worldPos, float radius, float duration)
		{
			Vector3 vector = cam.WorldToViewportPoint(worldPos);
			float value = Mathf.Abs(cam.WorldToViewportPoint(worldPos + new Vector3(radius, 0f, 0f)).x - vector.x);
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetVector(CenterID, vector);
			mpb.SetFloat(RadiusID, value);
			spriteRenderer.SetPropertyBlock(mpb);
			spriteRenderer.enabled = true;
			ps.transform.position = worldPos;
			ps.Play();
			waveTween.Stop();
			waveTween = Tween.Custom(-0.1f, 1f, duration, delegate(float v)
			{
				spriteRenderer.GetPropertyBlock(mpb);
				mpb.SetFloat(WaveDistanceID, v);
				spriteRenderer.SetPropertyBlock(mpb);
			}).OnComplete(this, delegate(ShockWaveController t)
			{
				t.spriteRenderer.enabled = false;
			});
		}
	}
}
