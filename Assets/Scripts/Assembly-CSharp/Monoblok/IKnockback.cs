using UnityEngine;

namespace Monoblok
{
	public interface IKnockback
	{
		void ApplyKnockback(Vector3 direction, float force);
	}
}
