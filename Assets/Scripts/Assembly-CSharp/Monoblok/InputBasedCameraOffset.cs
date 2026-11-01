using Monoblok.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace Monoblok
{
	public class InputBasedCameraOffset : MonoBehaviour
	{
		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private CinemachinePositionComposer composer;

		[SerializeField]
		[Range(0f, 1f)]
		private float mult;

		private void Update()
		{
			Vector2 droneMove = inputReader.DroneMove;
			composer.TargetOffset = droneMove * mult;
		}
	}
}
