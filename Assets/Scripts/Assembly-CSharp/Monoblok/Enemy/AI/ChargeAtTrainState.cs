using Monoblok.Movement;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Enemy.AI
{
	public class ChargeAtTrainState : EnemyState
	{
		private readonly ChargeAtTrainStateConfig config;

		private bool reached;

		private bool isCharging;

		private Vector2 origin;

		private float distance;

		private Vector2 direction;

		private ChargeSignal signal;

		private Tween windupTween;

		public ChargeAtTrainState(EnemyBrain brain, ChargeAtTrainStateConfig config)
			: base(brain, config)
		{
			this.config = config;
		}

		protected override void OnEnter()
		{
			reached = false;
			isCharging = false;
			signal = Object.Instantiate(config.SignalPrefab);
			signal.transform.position = brain.transform.position;
			distance = Vector2.Distance((Vector2)brain.transform.position, (Vector2)config.TrainRuntimeValue.Value.Bounds.center) + config.OverShoot;
			direction = (config.TrainRuntimeValue.Value.Bounds.center - brain.transform.position).normalized;
			signal.Initialize(distance, direction);
			windupTween = Tween.Delay(config.WindupDuration).OnComplete(this, delegate(ChargeAtTrainState t)
			{
				t.Charge();
			});
		}

		private void Charge()
		{
			origin = brain.transform.position;
			Vector2 vector = (Vector2)brain.transform.position + direction * distance;
			brain.Movement.SetDestination(new PositionTarget(vector));
			isCharging = true;
		}

		protected override void OnExit()
		{
			if (windupTween.isAlive)
			{
				windupTween.Stop();
			}
			Object.Destroy(signal.gameObject);
			brain.Movement.Stop();
		}

		protected override void OnUpdate(float deltaTime)
		{
			if (isCharging && !reached && Vector2.Distance((Vector2)brain.transform.position, origin) >= distance)
			{
				Finish();
				reached = true;
				isCharging = false;
			}
		}
	}
}
