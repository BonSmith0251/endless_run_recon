namespace Monoblok.Core
{
	public static class DescriptionFormatting
	{
		public enum ValueFormat
		{
			Plain,
			Percent,
			Seconds
		}

		public const string PositiveColor = "#71FF90";

		public static string GetColoredValue(float value, ValueFormat format)
		{
			return "<color=#71FF90>" + format switch
			{
				ValueFormat.Percent => (value * 100f).ToString("+0.##;-0.##") + "%", 
				ValueFormat.Seconds => value.ToString("+0.##;-0.##") + " sec", 
				_ => value.ToString("+0.##;-0.##"), 
			} + "</color>";
		}
	}
}
