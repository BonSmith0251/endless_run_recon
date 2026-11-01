using System.Collections.Generic;
using Monoblok.Input;
using Monoblok.StatSystem;
using PrimeTween;
using UnityEngine;

namespace Monoblok.DroneSystem
{
	public class DroneController : MonoBehaviour, IStatManagerInjectable
	{
		[SerializeField]
		private PlayerDrones playerDrones;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private StatOrValue speed;

		[Header("Play Area")]
		[SerializeField]
		private Vector3 center;

		[SerializeField]
		private Vector2 size;

		public void Inject(StatManager statManager)
		{
			speed.Bind(statManager);
		}

		private void Update()
		{
			Vector2 droneMove = inputReader.DroneMove;
			base.transform.position += (Vector3)droneMove * (float)speed * Time.deltaTime;
			base.transform.position = ClampToPlayArea(base.transform.position);
			IReadOnlyList<Drone> drones = playerDrones.Drones;
			for (int i = 0; i < drones.Count; i++)
			{
				drones[i].UpdateMoveAnimation(droneMove);
			}
		}

		public void PerformDash(float dashDistance)
		{
			Vector3 vector = inputReader.DroneMove.normalized;
			if (vector.x == 0f)
			{
				vector.x = 1f;
			}
			Vector3 endValue = ClampToPlayArea(base.transform.position + vector * dashDistance);
			Tween.Position(base.transform, endValue, 0.15f);
			IReadOnlyList<Drone> drones = playerDrones.Drones;
			for (int i = 0; i < drones.Count; i++)
			{
				drones[i].TriggerDashAnimation();
			}
		}

		private Vector3 ClampToPlayArea(Vector3 position)
		{
			position.x = Mathf.Clamp(position.x, center.x - size.x / 2f, center.x + size.x / 2f);
			position.y = Mathf.Clamp(position.y, center.y - size.y / 2f, center.y + size.y / 2f);
			return position;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(center, size);
		}
	}
}
