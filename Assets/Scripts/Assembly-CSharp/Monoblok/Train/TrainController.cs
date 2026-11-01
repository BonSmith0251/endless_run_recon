using System;
using Monoblok.Environment;
using Monoblok.Input;
using Monoblok.Isometric;
using Monoblok.SA;
using Monoblok.StatSystem;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;

namespace Monoblok.Train
{
	public class TrainController : MonoBehaviour, IStatManagerInjectable
	{
		[SerializeField]
		private Train train;

		[SerializeField]
		private BoolRuntimeValue canDrive;

		[SerializeField]
		private InputReader inputReader;

		[SerializeField]
		private StatOrValue speed;

		[SerializeField]
		private float maxDistance = 20f;

		[SerializeField]
		private ParticleSystem breakParticles;

		[Inject]
		private TracksManager tracksManager;

		private bool enableControls = true;

		private Vector3 startPos;

		private int tracksIndex = 1;

		private bool isBraking;

		private bool isAccelerating;

		public Vector3 Velocity { get; private set; }

		public event Action OnBrakeStarted;

		public event Action OnBrakeEnded;

		public event Action OnAccelerationStarted;

		public event Action OnAccelerationEnded;

		public void Inject(StatManager statManager)
		{
			speed.Bind(statManager);
		}

		private void Start()
		{
			startPos = base.transform.position;
			tracksIndex = 1;
			TrainTracks tracksAtIndex = tracksManager.GetTracksAtIndex(tracksIndex);
			Jump(tracksAtIndex);
		}

		private void Update()
		{
			if (!(canDrive != null) || canDrive.Value)
			{
				if (!enableControls || inputReader == null)
				{
					Velocity = Vector3.zero;
					StopBraking();
					StopAccelerating();
				}
				else
				{
					float trainMove = inputReader.TrainMove;
					UpdateBrakeState(trainMove);
					UpdateAccelerationState(trainMove);
					UpdateMovement(trainMove);
					UpdateTrackJump();
				}
			}
		}

		private void UpdateBrakeState(float input)
		{
			bool flag = input < 0f;
			if (flag && !isBraking)
			{
				isBraking = true;
				this.OnBrakeStarted?.Invoke();
				breakParticles.Play();
			}
			else if (!flag && isBraking)
			{
				StopBraking();
			}
		}

		private void StopBraking()
		{
			if (isBraking)
			{
				isBraking = false;
				this.OnBrakeEnded?.Invoke();
				breakParticles.Stop();
			}
		}

		private void UpdateAccelerationState(float input)
		{
			bool flag = input > 0f;
			if (flag && !isAccelerating)
			{
				isAccelerating = true;
				this.OnAccelerationStarted?.Invoke();
			}
			else if (!flag && isAccelerating)
			{
				StopAccelerating();
			}
		}

		private void StopAccelerating()
		{
			if (isAccelerating)
			{
				isAccelerating = false;
				this.OnAccelerationEnded?.Invoke();
			}
		}

		private void UpdateMovement(float input)
		{
			Vector3 position = base.transform.position;
			float num = input * (float)speed * Time.deltaTime;
			float num2 = Vector3.Dot(base.transform.position - startPos, (Vector3)Iso.Right);
			float num3 = Vector3.Dot(train.Bounds.extents, (Vector3)Iso.Right);
			float num4 = Vector3.Dot(train.Bounds.center - startPos, (Vector3)Iso.Right) - num2;
			num2 = Mathf.Clamp(num2 + num, 0f - maxDistance - num4 + num3, maxDistance - num4 - num3);
			base.transform.position = startPos + (Vector3)Iso.Right * num2;
			Velocity = (base.transform.position - position) / Time.deltaTime;
		}

		private void UpdateTrackJump()
		{
			if (inputReader.JumpDownPressed)
			{
				Jump(tracksManager.GetNextDown(tracksIndex, out var newIndex));
				tracksIndex = newIndex;
			}
			else if (inputReader.JumpUpPressed)
			{
				Jump(tracksManager.GetNextUp(tracksIndex, out var newIndex2));
				tracksIndex = newIndex2;
			}
		}

		private void Jump(TrainTracks tracks)
		{
			startPos = tracks.transform.position;
		}

		public void ResetPosition()
		{
			Tween.Position(base.transform, startPos, 2f, Ease.InOutCubic);
		}

		public void EnableControls(bool enabled)
		{
			enableControls = enabled;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Vector3 vector = startPos + (Vector3)Iso.Right * (0f - maxDistance);
			Vector3 vector2 = startPos + (Vector3)Iso.Right * maxDistance;
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawWireSphere(vector, 0.1f);
			Gizmos.DrawWireSphere(vector2, 0.1f);
		}
	}
}
