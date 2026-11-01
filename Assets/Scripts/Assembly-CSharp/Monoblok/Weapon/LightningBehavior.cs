using DigitalRuby.LightningBolt;
using Monoblok.DamageSystem;
using Monoblok.Movement;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Weapon
{
	public class LightningBehavior : IntervalWeaponBehavior
	{
		private const float BoltDuration = 0.1f;

		private readonly LightningWeaponConfig config;

		private readonly ObjectPool<LightningBoltScript> pool;

		private readonly Camera mainCam;

		private int layerMask;

		public LightningBehavior(Weapon weapon, LightningWeaponConfig config)
			: base(weapon, config)
		{
			this.config = config;
			pool = new ObjectPool<LightningBoltScript>(Create, OnGet, OnRelease, OnDestroy);
			mainCam = Camera.main;
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			base.RegisterStats(builder);
			layerMask = LayerMask.GetMask(LayerMask.LayerToName(weapon.layer));
		}

		private LightningBoltScript Create()
		{
			return Object.Instantiate(config.Prefab);
		}

		private void OnGet(LightningBoltScript instance)
		{
			instance.gameObject.SetActive(value: true);
			instance.Clear();
		}

		private void OnRelease(LightningBoltScript instance)
		{
			instance.StartObject.transform.position = instance.transform.position;
			instance.EndObject.transform.position = instance.transform.position;
			instance.gameObject.SetActive(value: false);
		}

		private void OnDestroy(LightningBoltScript instance)
		{
			if (instance != null) Object.Destroy(instance.gameObject);
		}

		protected override void OnTick(ITarget target)
		{
			LightningBoltScript instance = pool.Get();
			float y = mainCam.transform.position.y + mainCam.orthographicSize + 1f;
			Vector3 position = new Vector3(target.Position.x, y, 0f);
			instance.StartObject.transform.position = position;
			instance.EndObject.transform.position = position;
			Sequence.Create().Chain(Tween.Position(instance.EndObject.transform, target.Position, 0.1f)).ChainCallback(this, delegate(LightningBehavior t)
			{
				_ = Physics2D.CircleCast(target.Position, 0.1f, Vector2.zero, 0f, layerMask).collider;
				t.DamageAt(instance);
			})
				.Chain(Tween.Delay(0.1f))
				.ChainCallback(this, delegate(LightningBehavior t)
				{
					t.pool.Release(instance);
				});
		}

		private void DamageAt(LightningBoltScript instance)
		{
			RaycastHit2D raycastHit2D = Physics2D.CircleCast(instance.EndObject.transform.position, 0.1f, Vector2.zero, 0f, layerMask);
			if ((bool)raycastHit2D && raycastHit2D.collider.TryGetComponent<Health>(out var component))
			{
				weapon.DealDamage(component, Damage.DamageType.Projectile);
			}
		}
	}
}
