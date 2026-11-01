using System.Collections.Generic;

namespace Monoblok.Train
{
	public class SmartString
	{
		private Dictionary<string, string> tokens = new Dictionary<string, string>();

		public void AddToken(string key, string value)
		{
			tokens[key] = value;
		}

		public string Replace(string text)
		{
			string text2 = text;
			foreach (KeyValuePair<string, string> token in tokens)
			{
				text2 = text2.Replace("{" + token.Key + "}", token.Value);
			}
			return text2;
		}
	}
}
