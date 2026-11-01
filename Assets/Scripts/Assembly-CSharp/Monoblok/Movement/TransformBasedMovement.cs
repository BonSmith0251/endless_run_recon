using Monoblok.StatSystem;
using UnityEngine;

namespace Monoblok.Movement
{
	public class TransformBasedMovement : MovementBase
	{
		[SerializeField]
		private StatOrValue speed;

		[SerializeField]
		private StatManager statManager;

		private ITarget target;

		public override void SetDestination(ITarget target)
		{
			this.target = target;
		}

		public override void Stop()
		{
			target = null;
		}

		private void Start()
		{
			speed.Bind(statManager);
		}

		private void Update()
		{
			if (target != null)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, target.Position, speed.Value * Time.deltaTime);
			}
		}
	}
}
