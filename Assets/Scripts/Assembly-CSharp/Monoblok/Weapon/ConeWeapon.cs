using System.Collections.Generic;
using Monoblok.DamageSystem;
using UnityEngine;

namespace Monoblok.Weapon
{
	public class ConeWeapon : MonoBehaviour
	{
		[SerializeField]
		private BoxCollider2D boxCollider;

		[SerializeField]
		private ParticleSystem ps;

		private bool enableCollisions;

		private int layer;

		private float currentRange;

		private readonly List<Health> targets = new List<Health>();

		public IReadOnlyList<Health> Targets => targets;

		public void Initialize(int layer)
		{
			this.layer = layer;
			targets.Clear();
		}

		public void Stop()
		{
			if (ps != null)
			{
				ps.Stop();
			}
			enableCollisions = false;
			currentRange = 0f;
			targets.Clear();
		}

		public void Fire(Vector3 direction, float range)
		{
			enableCollisions = true;
			if (!ps.isEmitting)
			{
				ps.Play();
				currentRange = 0f;
			}
			base.transform.up = direction;
			ParticleSystem.MainModule main = ps.main;
			main.startLifetime = range / main.startSpeed.constant;
			currentRange = Mathf.MoveTowards(currentRange, range, main.startSpeed.constant * Time.deltaTime);
			boxCollider.offset = new Vector2(0f, currentRange / 2f);
			boxCollider.size = new Vector2(boxCollider.size.x, currentRange);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (enableCollisions && collision.gameObject.layer == layer && collision.TryGetComponent<Health>(out var component) && !targets.Contains(component))
			{
				targets.Add(component);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer == layer && collision.TryGetComponent<Health>(out var component))
			{
				targets.Remove(component);
			}
		}
	}
}
