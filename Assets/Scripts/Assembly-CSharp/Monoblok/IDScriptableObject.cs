using NaughtyAttributes;
using UnityEngine;

namespace Monoblok
{
	public abstract class IDScriptableObject : ScriptableObject
	{
		[SerializeField]
		[ReadOnly]
		[AllowNesting]
		private string id;

		public string ID => id;

		protected virtual void OnValidate()
		{
		}

		[Button(null, EButtonEnableMode.Always)]
		[ContextMenu("Regenerate guid")]
		private void RegenerateGuid()
		{
		}
	}
}
