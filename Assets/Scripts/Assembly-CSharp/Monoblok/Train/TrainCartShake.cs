using Monoblok.Progression;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Train
{
	public class TrainCartShake : MonoBehaviour
	{
		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[Header("Rail Joints")]
		[SerializeField]
		private float railLength = 2.5f;

		[SerializeField]
		private float phaseOffsetScale = 1f;

		[Header("Impulse")]
		[SerializeField]
		private float bumpHeight = 0.08f;

		[SerializeField]
		private float bumpDuration = 0.18f;

		[SerializeField]
		private Ease riseEase = Ease.OutQuad;

		[SerializeField]
		private Ease fallEase = Ease.InQuad;

		private float _railDistance;

		private float _lastJointDistance;

		private Vector3 _originLocalPos;

		private bool _isBumping;

		private void Start()
		{
			_originLocalPos = base.transform.localPosition;
			float railDistance = Mathf.Abs(base.transform.position.x) * phaseOffsetScale % railLength;
			_railDistance = railDistance;
			_lastJointDistance = Mathf.Floor(_railDistance / railLength) * railLength;
		}

		private void Update()
		{
			float normalizedSpeed = progression.NormalizedSpeed;
			_railDistance += normalizedSpeed * Time.deltaTime;
			float num = _lastJointDistance + railLength;
			if (_railDistance >= num)
			{
				_lastJointDistance = num;
				TriggerBump();
			}
		}

		private void TriggerBump()
		{
			if (_isBumping)
			{
				Tween.StopAll(this);
				base.transform.localPosition = _originLocalPos;
			}
			_isBumping = true;
			float duration = bumpDuration * 0.5f;
			Vector3 endValue = _originLocalPos + Vector3.up * bumpHeight;
			Tween.LocalPosition(base.transform, endValue, duration, riseEase).Chain(Tween.LocalPosition(base.transform, _originLocalPos, duration, fallEase)).OnComplete(this, delegate(TrainCartShake target)
			{
				target._isBumping = false;
			});
		}
	}
}
