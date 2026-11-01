using UnityEngine;

namespace Monoblok.StatSystem
{
	public static class StatDefinitionExtensions
	{
		public static string Format(this StatDefinition definition, float value)
		{
			string text = definition.StatValueType switch
			{
				StatValueType.Int => Mathf.RoundToInt(value).ToString(), 
				StatValueType.Percent => (value * 100f).ToString("0.#") + "%", 
				_ => value.ToString("0.##"), 
			};
			if (definition.MaxValue > 0f && value >= definition.MaxValue)
			{
				text += " (max)";
			}
			return text;
		}
	}
}
