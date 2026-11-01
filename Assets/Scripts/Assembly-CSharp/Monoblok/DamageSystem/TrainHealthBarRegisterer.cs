using Monoblok.Progression;
using Monoblok.SA;
using Monoblok.Train;
using PrimeTween;
using UnityEngine;

namespace Monoblok.DamageSystem
{
	public class TrainHealthBarRegisterer : MonoBehaviour
	{
		[SerializeField]
		private HealthRuntimeCollection collection;

		[SerializeField]
		private Monoblok.Progression.Progression progression;

		[SerializeField]
		private Monoblok.Train.Train train;

		[SerializeField]
		private float hideDelay = 2f;

		private Tween hideTween;

		private void OnEnable()
		{
			train.Health.OnDamaged += HandleDamaged;
			progression.OnJourneyStarted += HandleJourneyStarted;
			progression.OnJourneyEnded += HandleJourneyEnded;
		}

		private void OnDisable()
		{
			train.Health.OnDamaged -= HandleDamaged;
			progression.OnJourneyStarted -= HandleJourneyStarted;
			progression.OnJourneyEnded -= HandleJourneyEnded;
			if (hideTween.isAlive)
			{
				hideTween.Stop();
			}
		}

		private void HandleJourneyStarted()
		{
			collection.Unregister(train.Health);
		}

		private void HandleJourneyEnded()
		{
			hideTween.Stop();
			collection.Register(train.Health);
		}

		private void HandleDamaged(Damage _)
		{
			collection.Register(train.Health);
			if (hideTween.isAlive)
			{
				hideTween.Stop();
			}
			hideTween = Tween.Delay(hideDelay, delegate
			{
				collection.Unregister(train.Health);
			});
		}
	}
}
