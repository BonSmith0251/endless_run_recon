using PrimeTween;
using UnityEngine;

namespace Monoblok
{
	public class Knockback : MonoBehaviour, IKnockback
	{
		public void ApplyKnockback(Vector3 direction, float force)
		{
			Vector3 endValue = base.transform.position + direction * force;
			Tween.Position(base.transform, endValue, 0.15f);
			Debug.Log("Knockback applied to " + base.gameObject.name);
		}
	}
}
