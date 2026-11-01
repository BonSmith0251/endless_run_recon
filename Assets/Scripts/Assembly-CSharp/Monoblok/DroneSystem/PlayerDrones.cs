using System.Collections.Generic;
using Monoblok.Progression;
using Monoblok.StatSystem;
using PrimeTween;
using UnityEngine;

namespace Monoblok.DroneSystem
{
	public class PlayerDrones : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeReference]
		[SubclassSelector]
		private IDroneFormation formation;

		[SerializeField]
		private DroneController controller;

		[SerializeField]
		private Drone dronePrefab;

		[SerializeField]
		private Vector3 restingPlace = new Vector3(-5.5f, -0.5f, 0f);

		private List<Drone> drones = new List<Drone>();

		private Dictionary<Drone, Vector3> dronePositions = new Dictionary<Drone, Vector3>();

		public IReadOnlyList<Drone> Drones => drones;

		public DroneController Controller => controller;

		private void OnEnable()
		{
			progression.OnJourneyEnded += OnJourneyEnded;
		}

		private void OnDisable()
		{
			progression.OnJourneyEnded -= OnJourneyEnded;
		}

		private void Awake()
		{
			base.transform.position = restingPlace;
		}

		private void OnJourneyEnded()
		{
			Tween.Position(base.transform, restingPlace, 0.3f);
		}

		public void ClearDrones()
		{
			for (int num = drones.Count - 1; num >= 0; num--)
			{
				Drone drone = drones[num];
				RemoveDrone(drone);
			}
		}

		public void RemoveDrone(Drone drone)
		{
			if (drones.Remove(drone))
			{
				// Removal owns cancellation: the spawn callback cannot outlive its drone.
				Tween.StopAll(drone.transform);
				Object.Destroy(drone.gameObject);
				RecalculatePositions();
			}
		}

		public bool TryAddDrone(DroneData data, StatManager statManager, out Drone drone)
		{
			drone = Object.Instantiate(dronePrefab);
			drone.transform.position = base.transform.position;
			drone.transform.localScale = Vector3.zero;
			drone.transform.SetParent(base.transform, worldPositionStays: true);
			drone.Initialize(data, statManager);
			drones.Add(drone);
			Tween.Scale(drone.transform, 1f, 0.3f).OnComplete(this, delegate(PlayerDrones t)
			{
				t.RecalculatePositions();
			});
			return true;
		}

		private void RecalculatePositions()
		{
			Vector3[] positions = formation.GetPositions(base.transform.position, drones.Count);
			for (int i = 0; i < drones.Count; i++)
			{
				Drone drone = drones[i];
				Vector3 vector = positions[i];
				dronePositions[drone] = vector;
				Tween.Position(drone.transform, vector, 0.3f);
			}
		}

		public void ResetPosition()
		{
			base.transform.position = restingPlace;
		}
	}
}
