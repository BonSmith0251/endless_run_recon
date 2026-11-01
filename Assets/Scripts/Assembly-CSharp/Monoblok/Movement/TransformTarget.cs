using UnityEngine;

namespace Monoblok.Movement
{
	public class TransformTarget : ITarget
	{
		private Transform target;

		public Vector3 Position
		{
			get
			{
				if (!(target == null))
				{
					return target.position;
				}
				return default(Vector3);
			}
		}

		public TransformTarget(Transform target)
		{
			this.target = target;
		}

		public void UpdateTarget(Transform tr)
		{
			target = tr;
		}
	}
}
