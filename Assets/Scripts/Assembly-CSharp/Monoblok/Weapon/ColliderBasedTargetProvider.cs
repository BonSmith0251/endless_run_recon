using System.Collections.Generic;
using Monoblok.Movement;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok.Weapon
{
	[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
	public class ColliderBasedTargetProvider : WeaponTargetProvider
	{
		[SerializeField]
		private CircleCollider2D myCollider;

		[SerializeField]
		private Rigidbody2D rb;

		[SerializeField]
		[Layer]
		private int layer;

		[SerializeReference]
		[SubclassSelector]
		private ITargetFilter targetFilter;

		private readonly List<Transform> targetsInRange = new List<Transform>();

		private Transform currentTargetTransform;

		private TransformTarget cachedTarget;

		private void OnValidate()
		{
			if (myCollider == null)
			{
				myCollider = GetComponent<CircleCollider2D>();
			}
			if (myCollider != null)
			{
				myCollider.isTrigger = true;
			}
			if (rb == null)
			{
				rb = GetComponent<Rigidbody2D>();
				rb.bodyType = RigidbodyType2D.Kinematic;
			}
		}

		private void Awake()
		{
			if (myCollider == null)
			{
				myCollider = GetComponent<CircleCollider2D>();
			}
			myCollider.isTrigger = true;
			if (rb == null)
			{
				rb = GetComponent<Rigidbody2D>();
				rb.bodyType = RigidbodyType2D.Kinematic;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!(collision == null) && collision.gameObject.layer == layer)
			{
				targetsInRange.Add(collision.transform);
				RefreshTarget();
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (!(collision == null) && collision.gameObject.layer == layer)
			{
				targetsInRange.Remove(collision.transform);
				RefreshTarget();
			}
		}

		public override void OnRangeChanged(float newRange)
		{
			if (!(myCollider == null))
			{
				myCollider.radius = newRange;
			}
		}

		private void RefreshTarget()
		{
			if (targetFilter == null)
			{
				return;
			}
			Transform transform = targetFilter.GetTarget(base.transform, targetsInRange);
			if (transform == currentTargetTransform)
			{
				return;
			}
			currentTargetTransform = transform;
			if (transform == null)
			{
				target = null;
				return;
			}
			if (cachedTarget == null)
			{
				cachedTarget = new TransformTarget(transform);
			}
			else
			{
				cachedTarget.UpdateTarget(transform);
			}
			target = cachedTarget;
		}

		private void OnDrawGizmosSelected()
		{
			if (!(currentTargetTransform == null))
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(currentTargetTransform.position, 1f);
			}
		}
	}
}
