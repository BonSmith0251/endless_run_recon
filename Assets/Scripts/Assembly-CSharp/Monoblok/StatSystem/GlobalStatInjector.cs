using Monoblok.SA;
using UnityEngine;

namespace Monoblok.StatSystem
{
	public class GlobalStatInjector : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private GlobalStatManagerRuntimeValue runtimeValue;

		private void Start()
		{
			if (runtimeValue == null || runtimeValue.Value == null)
			{
				Debug.LogError("No global statmanager or runtimevalue assigned", this);
				return;
			}
			IStatManagerInjectable[] componentsInChildren = GetComponentsInChildren<IStatManagerInjectable>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Inject(runtimeValue.Value.StatManager);
			}
		}
	}
}
