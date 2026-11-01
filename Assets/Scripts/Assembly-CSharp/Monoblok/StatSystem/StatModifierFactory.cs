using System;

namespace Monoblok.StatSystem
{
	[Serializable]
	public class StatModifierFactory
	{
		public enum ModifierType
		{
			Additive,
			Multiplicative
		}

		public ModifierType Type;

		public float value;

		public bool displayAsPercent;

		public bool IsValid()
		{
			return Type switch
			{
				ModifierType.Additive => value != 0f, 
				ModifierType.Multiplicative => value != 1f, 
				_ => throw new NotImplementedException("Modifier type not supported"), 
			};
		}

		public StatModifier GetModifier(object source)
		{
			return Type switch
			{
				ModifierType.Additive => new AdditiveModifier(source, value), 
				ModifierType.Multiplicative => new MultiplicativeModifier(source, value), 
				_ => throw new NotImplementedException("Modifier type not supported"), 
			};
		}

		public override string ToString()
		{
			return Type switch
			{
				ModifierType.Additive => displayAsPercent ? ((value * 100f).ToString("+0.##;-0.##") + "%") : value.ToString("+0.##;-0.##"), 
				ModifierType.Multiplicative => ((value - 1f) * 100f).ToString("+0.##;-0.##") + "%", 
				_ => "Unknown(" + value + ")", 
			};
		}

		public string GetColoredString()
		{
			string arg = ((GetMagnitude() >= 0f) ? "71FF90" : "FF4E4E");
			return $"<color=#{arg}>{this}</color>";
		}

		public float GetMagnitude()
		{
			return Type switch
			{
				ModifierType.Additive => value, 
				ModifierType.Multiplicative => value - 1f, 
				_ => throw new NotImplementedException(), 
			};
		}

		public StatValueType GetValueType()
		{
			return Type switch
			{
				ModifierType.Additive => StatValueType.Regular, 
				ModifierType.Multiplicative => StatValueType.Percent, 
				_ => throw new NotImplementedException(), 
			};
		}

		public object GetDescriptionValue()
		{
			if (Type != ModifierType.Multiplicative && !displayAsPercent)
			{
				return Math.Abs(GetMagnitude());
			}
			return Math.Abs(GetMagnitude() * 100f).ToString("0.##") + "%";
		}

		public StatModifier GetScaledModifier(object source, float magnitude)
		{
			return Type switch
			{
				ModifierType.Additive => new AdditiveModifier(source, magnitude), 
				ModifierType.Multiplicative => new MultiplicativeModifier(source, 1f + magnitude), 
				_ => throw new NotImplementedException(), 
			};
		}
	}
}
