using FMODUnity;
using Monoblok.Inventory;
using Monoblok.SA;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

namespace Monoblok
{
	public class CoinCollectable : Collectable
	{
		[SerializeField]
		private Collider2D myCollider;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private PlayerWallet wallet;

		[SerializeField]
		private CurrencyDefinition currency;

		[SerializeField]
		private int amount;

		[SerializeField]
		private GameEvent collectedEvent;

		[Header("Expiration")]
		[SerializeField]
		[Min(0f)]
		private float blinkDuration = 1f;

		[SerializeField]
		[Min(1f)]
		private int blinkCount = 6;

		[SerializeField]
		[Range(0f, 1f)]
		private float blinkMinAlpha = 0.4f;

		[SerializeField]
		[Min(0.01f)]
		private float scaleDownDuration = 0.3f;

		[SerializeField]
		private EventReference pickupRef;

		private ObjectPool<CoinCollectable> pool;

		private bool isReleased;

		private bool lifetimeActive;

		private float lifetimeTimer;

		private Sequence expireSequence;

		private void OnValidate()
		{
			if (myCollider != null)
			{
				myCollider = GetComponent<Collider2D>();
			}
			if (spriteRenderer == null)
			{
				spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			}
		}

		public void EnableCollider(bool enable)
		{
			myCollider.enabled = enable;
		}

		public void Initialize(ObjectPool<CoinCollectable> pool, float lifetime)
		{
			this.pool = pool;
			isReleased = false;
			base.transform.localScale = Vector3.one;
			lifetimeActive = false;
			if (expireSequence.isAlive)
			{
				expireSequence.Stop();
			}
			SetAlpha(1f);
			if (lifetime > 0f)
			{
				lifetimeTimer = lifetime;
				lifetimeActive = true;
			}
		}

		public void CancelExpiration()
		{
			lifetimeActive = false;
			if (expireSequence.isAlive)
			{
				expireSequence.Stop();
			}
			SetAlpha(1f);
		}

		private void Update()
		{
			if (lifetimeActive)
			{
				lifetimeTimer -= Time.deltaTime;
				if (lifetimeTimer <= 0f)
				{
					lifetimeActive = false;
					Expire();
				}
			}
		}

		private void SetAlpha(float alpha)
		{
			Color color = spriteRenderer.color;
			color.a = alpha;
			spriteRenderer.color = color;
		}

		public override void Collect(Collector collector)
		{
			if (collectedEvent != null)
			{
				collectedEvent.TriggerGameEvent();
			}
			wallet.AddCurrency(currency, amount);
			if (!pickupRef.IsNull)
			{
				RuntimeManager.PlayOneShot(pickupRef);
			}
			ReleaseToPool();
		}

		private void Expire()
		{
			if (!isReleased)
			{
				ReleaseToPool();
			}
		}

		public void ReleaseToPool()
		{
			if (!isReleased)
			{
				isReleased = true;
				lifetimeActive = false;
				if (expireSequence.isAlive)
				{
					expireSequence.Stop();
				}
				if (pool != null)
				{
					ObjectPool<CoinCollectable> objectPool = pool;
					pool = null;
					objectPool.Release(this);
				}
				else
				{
					Object.Destroy(base.gameObject);
				}
			}
		}
	}
}
