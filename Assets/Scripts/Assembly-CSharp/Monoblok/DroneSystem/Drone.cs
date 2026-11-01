using Monoblok.StatSystem;
using Monoblok.Weapon;
using PrimeTween;
using UnityEngine;

namespace Monoblok.DroneSystem
{
	public class Drone : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private WeaponHandler weaponHandler;

		[SerializeField]
		private ParticleSystem movePs;

		[Header("Config")]
		[SerializeField]
		private float maxTiltAngle = 25f;

		[SerializeField]
		private float tiltSpeed = 8f;

		private MaterialPropertyBlock mpb;

		private static readonly int GLOWID = Shader.PropertyToID("_GlowGlobal");

		private static readonly int GLOWCOLORID = Shader.PropertyToID("_GlowColor");

		private static readonly int GLOWCOLORINTENSITYID = Shader.PropertyToID("_Glow");

		private Tween statBoostGlowTween;

		public WeaponHandler WeaponHandler => weaponHandler;

		private void OnValidate()
		{
			if (weaponHandler == null)
			{
				weaponHandler = GetComponentInChildren<WeaponHandler>();
			}
			if (spriteRenderer == null)
			{
				spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			}
		}

		private void Awake()
		{
			mpb = new MaterialPropertyBlock();
		}

		public void Initialize(DroneData droneData, StatManager statManager)
		{
			spriteRenderer.sprite = droneData.DroneSprite;
			weaponHandler.RegisterStatManager(statManager);
			weaponHandler.Initialize(droneData.WeaponConfig);
		}

		public void TriggerDashAnimation()
		{
			spriteRenderer.GetPropertyBlock(mpb);
			Tween.Custom(0f, 1f, 0.15f, delegate(float v)
			{
				float value = 1f + (1f - Mathf.Abs(v * 2f - 1f)) * 20f;
				mpb.SetFloat(GLOWID, value);
				spriteRenderer.SetPropertyBlock(mpb);
			});
		}

		public void TriggerStatBoostAnimation(float dur)
		{
			statBoostGlowTween.Stop();
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetColor(GLOWCOLORID, Color.red);
			mpb.SetFloat(GLOWCOLORINTENSITYID, 20f);
			spriteRenderer.SetPropertyBlock(mpb);
			statBoostGlowTween = Tween.Custom(this, 0f, 5f, 0.3f, delegate(Drone drone, float v)
			{
				drone.spriteRenderer.GetPropertyBlock(drone.mpb);
				drone.mpb.SetFloat(GLOWID, v);
				drone.spriteRenderer.SetPropertyBlock(drone.mpb);
			}, default(Easing), -1, CycleMode.Yoyo);
			Tween.Delay(this, dur, delegate(Drone drone)
			{
				drone.statBoostGlowTween.Stop();
				drone.spriteRenderer.GetPropertyBlock(drone.mpb);
				drone.mpb.SetFloat(GLOWID, 1f);
				drone.mpb.SetColor(GLOWCOLORID, Color.white);
				drone.mpb.SetFloat(GLOWCOLORINTENSITYID, 0f);
				drone.spriteRenderer.SetPropertyBlock(drone.mpb);
			});
		}

		public void StopStatBoostAnimation()
		{
			statBoostGlowTween.Stop();
			spriteRenderer.GetPropertyBlock(mpb);
			mpb.SetFloat(GLOWID, 1f);
			mpb.SetColor(GLOWCOLORID, Color.white);
			mpb.SetFloat(GLOWCOLORINTENSITYID, 0f);
			spriteRenderer.SetPropertyBlock(mpb);
		}

		public void UpdateMoveAnimation(Vector2 move)
		{
			float b = (0f - move.x) * maxTiltAngle;
			float num = spriteRenderer.transform.localEulerAngles.z;
			if (num > 180f)
			{
				num -= 360f;
			}
			float z = Mathf.LerpAngle(num, b, Time.deltaTime * tiltSpeed);
			spriteRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, z);
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = movePs.velocityOverLifetime;
			float num2 = Mathf.Max(move.x, 0.1f);
			velocityOverLifetime.speedModifier = num2;
		}
	}
}
