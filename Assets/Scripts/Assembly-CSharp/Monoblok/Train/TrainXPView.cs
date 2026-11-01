using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monoblok.Train
{
	public class TrainXPView : MonoBehaviour
	{
		[SerializeField]
		private Slider slider;

		[SerializeField]
		private TMP_Text levelText;

		[SerializeField]
		private TMP_Text currentPerMaxText;

		[SerializeField]
		private TrainExperience experience;

		[Header("Config")]
		[SerializeField]
		private TweenSettings sliderSettings;

		[SerializeField]
		private ShakeSettings levelTextSettings;

		private Tween sliderTween;

		private void OnEnable()
		{
			experience.OnXPGained += UpdateXP;
			experience.OnLevelUp += OnLevelUp;
			levelText.text = experience.CurrentLevel.ToString();
			slider.maxValue = experience.XPToNextLevel;
			slider.value = experience.CurrentLevelXP;
			currentPerMaxText.text = experience.CurrentLevelXP + "/" + experience.XPToNextLevel;
		}

		private void OnDisable()
		{
			experience.OnXPGained -= UpdateXP;
			experience.OnLevelUp -= OnLevelUp;
		}

		private void UpdateXP()
		{
			int currentLevelXP = experience.CurrentLevelXP;
			sliderTween = Tween.UISliderValue(slider, new TweenSettings<float>(currentLevelXP, sliderSettings));
			currentPerMaxText.text = experience.CurrentLevelXP + "/" + experience.XPToNextLevel;
		}

		private void OnLevelUp()
		{
			sliderTween.Complete();
			slider.value = 0f;
			slider.maxValue = experience.XPToNextLevel;
			levelText.text = experience.CurrentLevel.ToString();
			Tween.PunchScale(levelText.transform, levelTextSettings);
		}
	}
}
