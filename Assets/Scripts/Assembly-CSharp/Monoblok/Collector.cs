using System.Collections.Generic;
using Monoblok.StatSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Monoblok
{
	[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
	public class Collector : MonoBehaviour, IStatManagerInjectable
	{
		private readonly struct CollectingState
		{
			public readonly Vector3 StartPosition;

			public readonly float StartTime;

			public CollectingState(Vector3 startPosition, float startTime)
			{
				StartPosition = startPosition;
				StartTime = startTime;
			}
		}

		[SerializeField]
		[Layer]
		private int layer;

		[SerializeField]
		private float collectDuration = 0.3f;

		[SerializeField]
		private AnimationCurve collectCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 1f, 2f, 0f));

		[SerializeField]
		private StatOrValue radius;

		private CircleCollider2D circleCollider;

		private readonly Dictionary<Collectable, CollectingState> collecting = new Dictionary<Collectable, CollectingState>();

		private readonly List<Collectable> finishedThisFrame = new List<Collectable>();

		private void Awake()
		{
			circleCollider = GetComponent<CircleCollider2D>();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == layer && collision.TryGetComponent<Collectable>(out var component) && !collecting.ContainsKey(component))
			{
				Debug.Log("Collecting");
				collecting[component] = new CollectingState(component.transform.position, Time.time);
			}
		}

		private void Update()
		{
			if (collecting.Count == 0)
			{
				return;
			}
			Vector3 position = base.transform.position;
			foreach (KeyValuePair<Collectable, CollectingState> item in collecting)
			{
				Collectable key = item.Key;
				CollectingState value = item.Value;
				if (!key)
				{
					finishedThisFrame.Add(key);
					continue;
				}
				float num = ((collectDuration > 0f) ? Mathf.Clamp01((Time.time - value.StartTime) / collectDuration) : 1f);
				key.transform.position = Vector3.LerpUnclamped(value.StartPosition, position, collectCurve.Evaluate(num));
				if (num >= 1f)
				{
					finishedThisFrame.Add(key);
				}
			}
			for (int i = 0; i < finishedThisFrame.Count; i++)
			{
				Collectable collectable = finishedThisFrame[i];
				collecting.Remove(collectable);
				if ((bool)collectable)
				{
					collectable.Collect(this);
				}
			}
			finishedThisFrame.Clear();
		}

		public void Inject(StatManager statManager)
		{
			radius.Bind(statManager, UpdateRadius);
			UpdateRadius(radius.Value);
		}

		private void UpdateRadius(float value)
		{
			circleCollider.radius = value;
		}
	}
}
