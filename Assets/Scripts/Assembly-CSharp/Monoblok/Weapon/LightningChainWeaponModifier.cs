using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using FMODUnity;
using Monoblok.DamageSystem;
using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok.Weapon
{
	public class LightningChainWeaponModifier : WeaponModifier
	{
		private struct ActiveBolt
		{
			public LightningBoltScript Instance;

			public Vector3 From;

			public Vector3 To;

			public float Elapsed;
		}

		private readonly LightningChainWeaponModifierDefinition config;

		private readonly ObjectPool<LightningBoltScript> pool;

		private readonly List<ActiveBolt> activeBolts = new List<ActiveBolt>();

		private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

		private Stat chainCount;

		private Stat chainRange;

		private Stat damage;

		private int layerMask;

		public LightningChainWeaponModifier(Weapon weapon, LightningChainWeaponModifierDefinition config)
			: base(weapon)
		{
			this.config = config;
			pool = new ObjectPool<LightningBoltScript>(Create, OnGet, OnRelease, OnDestroy);
		}

		internal override void RegisterStats(WeaponStatBuilder builder)
		{
			builder.Add(config.ChainCount, out chainCount);
			builder.Add(config.ChainRange, out chainRange);
			builder.Add(config.Damage, out damage);
			layerMask = LayerMask.GetMask(LayerMask.LayerToName(weapon.layer));
		}

		internal override void UnregisterStats(WeaponStatBuilder builder)
		{
			builder.Remove(config.ChainCount);
			builder.Remove(config.ChainRange);
			builder.Remove(config.Damage);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			for (int i = 0; i < activeBolts.Count; i++)
			{
				pool.Release(activeBolts[i].Instance);
			}
			activeBolts.Clear();
			pool.Clear();
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
			instance.gameObject.SetActive(value: false);
		}

		private void OnDestroy(LightningBoltScript instance)
		{
			if (instance != null) Object.Destroy(instance.gameObject);
		}

		protected override void HandleHit(IDamageable target, Damage damage)
		{
			if (!(target is Component component))
			{
				return;
			}
			int num = (int)chainCount.Value;
			if (num <= 0)
			{
				return;
			}
			Vector2 vector = component.transform.position;
			int num2 = Physics2D.CircleCastNonAlloc(vector, chainRange.Value, Vector2.zero, hitBuffer, 0f, layerMask);
			int num3 = 0;
			bool flag = false;
			for (int i = 0; i < num2; i++)
			{
				if (num3 >= num)
				{
					break;
				}
				RaycastHit2D raycastHit2D = hitBuffer[i];
				if (!(raycastHit2D.collider.gameObject == component.gameObject) && raycastHit2D.collider.TryGetComponent<IDamageable>(out var component2))
				{
					SpawnBolt(vector, raycastHit2D.collider.transform.position);
					component2.Damage(new Damage((int)this.damage.Value, 0, Damage.DamageType.Projectile));
					if (!flag)
					{
						RuntimeManager.PlayOneShot(config.ChainRef);
						flag = true;
					}
					num3++;
				}
			}
		}

		public override void Update(float deltaTime)
		{
			float boltDuration = config.BoltDuration;
			float num = boltDuration * 2f;
			for (int num2 = activeBolts.Count - 1; num2 >= 0; num2--)
			{
				ActiveBolt value = activeBolts[num2];
				value.Elapsed += deltaTime;
				float t = ((boltDuration > 0f) ? Mathf.Clamp01(value.Elapsed / boltDuration) : 1f);
				value.Instance.EndObject.transform.position = Vector3.Lerp(value.From, value.To, t);
				if (value.Elapsed >= num)
				{
					pool.Release(value.Instance);
					activeBolts.RemoveAt(num2);
				}
				else
				{
					activeBolts[num2] = value;
				}
			}
		}

		private void SpawnBolt(Vector2 from, Vector2 to)
		{
			LightningBoltScript lightningBoltScript = pool.Get();
			Vector3 vector = new Vector3(from.x, from.y, 0f);
			Vector3 to2 = new Vector3(to.x, to.y, 0f);
			lightningBoltScript.StartObject.transform.position = vector;
			lightningBoltScript.EndObject.transform.position = vector;
			activeBolts.Add(new ActiveBolt
			{
				Instance = lightningBoltScript,
				From = vector,
				To = to2,
				Elapsed = 0f
			});
		}
	}
}
