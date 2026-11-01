using FMODUnity;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Train
{
	public class TrainExplosion : MonoBehaviour
	{
		[SerializeField]
		private Train train;

		[SerializeField]
		private EventReference explosionRef;

		[Header("Push")]
		[SerializeField]
		private float pushRadius = 6f;

		[SerializeField]
		private float pushDuration = 0.35f;

		[SerializeField]
		private Ease pushEase = Ease.OutCubic;

		[Header("Rotation")]
		[SerializeField]
		private float maxRotationAngle = 45f;

		[SerializeField]
		private float rotateDuration = 0.35f;

		[SerializeField]
		private Ease rotateEase = Ease.OutCubic;

		[Button(null, EButtonEnableMode.Always)]
		public void Explode()
		{
			Vector2 origin = base.transform.position;
			Sequence sequence = Sequence.Create();
			foreach (TrainCart cart in train.Carts)
			{
				sequence.ChainCallback(this, delegate(TrainExplosion t)
				{
					t.Explode(cart);
				}).ChainDelay(0.1f);
				AnimateCart(cart, origin);
			}
		}

		private void Explode(TrainCart cart)
		{
			cart.ExplosionParticles.Play();
			RuntimeManager.PlayOneShot(explosionRef);
		}

		private void AnimateCart(TrainCart cart, Vector2 origin)
		{
			Transform transform = cart.transform;
			Vector2 vector = (Vector2)transform.position - origin;
			vector.y += Random.Range(-0.8f, 0.8f);
			vector.Normalize();
			float num = pushRadius * Random.Range(0.6f, 1.4f);
			Vector3 endValue = transform.position + (Vector3)(vector * num);
			float num2 = ((vector.x >= 0f) ? 1f : (-1f));
			float num3 = maxRotationAngle * Random.Range(0.4f, 1.6f) * num2;
			Quaternion endValue2 = transform.rotation * Quaternion.Euler(0f, 0f, 0f - num3);
			float num4 = Random.Range(0.85f, 1.15f);
			Sequence.Create().Group(Tween.Position(transform, endValue, pushDuration * num4, pushEase)).Group(Tween.Rotation(transform, endValue2, rotateDuration * num4, rotateEase));
		}
	}
}
