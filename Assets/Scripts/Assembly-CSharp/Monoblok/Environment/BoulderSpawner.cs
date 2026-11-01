using System.Collections.Generic;
using Monoblok.Isometric;
using Monoblok.Progression;
using Reflex.Attributes;
using Unity.Cinemachine;
using UnityEngine;

namespace Monoblok.Environment
{
	public class BoulderSpawner : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private Boulder boulderPrefab;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[Header("Config")]
		[SerializeField]
		[MinMaxRangeSlider(1f, 30f)]
		private Vector2 minMaxSpawnInterval;

		[SerializeField]
		private float spawnDistance = 20f;

		[Inject]
		private TracksManager tracksManager;

		[Inject]
		private EnvironmentController environmentController;

		[Inject]
		private DangerManager dangerManager;

		private bool shouldSpawn;

		private float currentInterval;

		private float t;

		private List<Boulder> boulders = new List<Boulder>();

		private void OnEnable()
		{
			progression.OnWaveStarted += OnWaveStarted;
			progression.OnWaveEnded += OnWaveEnded;
		}

		private void OnDisable()
		{
			progression.OnWaveStarted -= OnWaveStarted;
			progression.OnWaveEnded -= OnWaveEnded;
		}

		private void OnWaveStarted()
		{
			shouldSpawn = true;
			t = 0f;
			currentInterval = Random.Range(minMaxSpawnInterval.x, minMaxSpawnInterval.y);
		}

		private void OnWaveEnded()
		{
			shouldSpawn = false;
			for (int num = boulders.Count - 1; num >= 0; num--)
			{
				Boulder boulder = boulders[num];
				DestroyBoulder(boulder);
			}
			boulders.Clear();
		}

		private void Update()
		{
			if (shouldSpawn)
			{
				t += Time.deltaTime;
				if (!(t < currentInterval))
				{
					Spawn();
					t = 0f;
				}
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.TryGetComponent<Boulder>(out var component))
			{
				DestroyBoulder(component);
			}
		}

		private void DestroyBoulder(Boulder boulder)
		{
			boulders.Remove(boulder);
			dangerManager.RemoveDanger(boulder.transform);
			Object.Destroy(boulder.gameObject);
		}

		private void Spawn()
		{
			int index = Random.Range(0, tracksManager.Tracks.Count);
			TrainTracks trainTracks = tracksManager.Tracks[index];
			Vector3 position = trainTracks.transform.position + (Vector3)Iso.Right * spawnDistance;
			Boulder boulder = Object.Instantiate(boulderPrefab, base.transform);
			boulder.Initialize(position, trainTracks, progression.GlobalDistance);
			environmentController.Register(boulder);
			dangerManager.RegisterDanger(boulder.transform, trainTracks);
			boulders.Add(boulder);
		}
	}
}
