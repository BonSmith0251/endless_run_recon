using System;
using Animancer;
using FMODUnity;
using Monoblok.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Progression
{
	public class BossIntroduction : MonoBehaviour
	{
		[SerializeField]
		private RectTransform container;

		[SerializeField]
		private TMP_Text bossNameText;

		[SerializeField]
		private Image bossImage;

		[SerializeField]
		private AnimancerComponent animancer;

		[SerializeField]
		private AnimationClip clip;

		[SerializeField]
		private EventReference eventRef;

		public event Action OnComplete;

		public void StartIntroduction(EnemyConfig config)
		{
			bossNameText.text = config.EnemyName;
			bossImage.sprite = config.Prefab.SpriteRenderer.sprite;
			container.gameObject.SetActive(value: true);
			RuntimeManager.PlayOneShot(eventRef);
			animancer.Play(clip).OwnedEvents.EndEvent.callback = OnAnimationComplete;
		}

		private void OnAnimationComplete()
		{
			animancer.Stop();
			container.gameObject.SetActive(value: false);
			this.OnComplete?.Invoke();
		}

		public void Cancel()
		{
			animancer.Stop();
			container.gameObject.SetActive(value: false);
		}
	}
}
