using System.Collections.Generic;
using Monoblok.SA;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class HealthViewManager : MonoBehaviour
	{
		[SerializeField]
		private HealthBarView healthBarViewPrefab;

		[SerializeField]
		private HealthRuntimeCollection observedHealths;

		private Camera mainCam;

		private Dictionary<Health, HealthBarView> map = new Dictionary<Health, HealthBarView>();

		private void Awake()
		{
			mainCam = Camera.main;
			observedHealths.OnAdded += OnAdded;
			observedHealths.OnRemoved += OnRemovedHandler;
			foreach (Health item in observedHealths.Collection)
			{
				OnAdded(item);
			}
		}

		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				observedHealths.OnAdded -= OnAdded;
				observedHealths.OnRemoved -= OnRemovedHandler;
			}
		}

		private void OnAdded(Health health)
		{
			if (!(health == null) && !map.ContainsKey(health))
			{
				HealthBarView healthBarView = Object.Instantiate(healthBarViewPrefab, base.transform);
				healthBarView.Initialize(health);
				map.Add(health, healthBarView);
			}
		}

		private void OnRemovedHandler(Health health)
		{
			OnRemoved(health);
		}

		private void OnRemoved(Health health)
		{
			if (map.TryGetValue(health, out var value))
			{
				map.Remove(health);
				Object.Destroy(value.gameObject);
			}
		}

		private void LateUpdate()
		{
			foreach (KeyValuePair<Health, HealthBarView> item in map)
			{
				item.Value.UpdatePosition(mainCam);
			}
		}
	}
}
