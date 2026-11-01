using System.Linq;
using UnityEngine;

namespace Monoblok.Train
{
	[CreateAssetMenu(menuName = "Game/Train/Modules/Buffer")]
	public class BufferModuleDefinition : ModuleDefinition
	{
		[SerializeField]
		[HideInInspector]
		private BufferModuleBehavior prefab;

		[SerializeField]
		private CartBufferRule[] rules;

		public override string Description
		{
			get
			{
				if (rules.Length == 0)
				{
					return base.Description;
				}
				return string.Join(" ", rules.Select((CartBufferRule r) => r.GetDescription()));
			}
		}

		public override ModuleBehavior CreateBehavior(Transform parent)
		{
			BufferModuleBehavior bufferModuleBehavior = Object.Instantiate(prefab, parent);
			bufferModuleBehavior.Initialize(base.Sprite, rules);
			return bufferModuleBehavior;
		}
	}
}
