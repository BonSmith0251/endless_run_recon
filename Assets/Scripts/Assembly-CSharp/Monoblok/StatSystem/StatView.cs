using TMPro;
using UnityEngine;

namespace Monoblok.StatSystem
{
	public class StatView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text statNameText;

		[SerializeField]
		private TMP_Text baseValueText;

		[SerializeField]
		private TMP_Text valueText;

		[SerializeField]
		private TMP_Text arrow;

		[SerializeField]
		private Color baseColor;

		[SerializeField]
		private Color positiveColor;

		[SerializeField]
		private Color negativeColor;

		public Stat Stat { get; private set; }

		private void OnDestroy()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (Stat != null)
			{
				Stat.OnValueChanged -= OnStatValueChanged;
				Stat.definition.LocalizedName.StringChanged -= SetName;
			}
		}

		public void Initialize(Stat stat)
		{
			Stat = stat;
			SetName(stat.definition.Name);
			stat.definition.LocalizedName.StringChanged += SetName;
			SetBaseValueText(stat.BaseValue);
			stat.OnValueChanged += OnStatValueChanged;
			UpdateValueText();
		}

		private void SetName(string name)
		{
			statNameText.text = name;
		}

		private void OnStatValueChanged(float _)
		{
			SetBaseValueText(Stat.BaseValue);
			UpdateValueText();
		}

		private void SetBaseValueText(float baseValue)
		{
			if (!(baseValueText == null))
			{
				baseValueText.text = Stat.definition.Format(baseValue);
			}
		}

		private void UpdateValueText()
		{
			if (Stat != null)
			{
				bool flag = Stat.BaseValue != Stat.Value;
				arrow.gameObject.SetActive(flag);
				valueText.gameObject.SetActive(flag);
				if (flag)
				{
					valueText.text = Stat.definition.Format(Stat.Value);
					valueText.color = GetColor();
					arrow.color = GetColor();
				}
			}
		}

		private Color GetColor()
		{
			if (Stat == null)
			{
				return baseColor;
			}
			if (Stat.BaseValue < Stat.Value)
			{
				return positiveColor;
			}
			if (Stat.BaseValue > Stat.Value)
			{
				return negativeColor;
			}
			return baseColor;
		}
	}
}
