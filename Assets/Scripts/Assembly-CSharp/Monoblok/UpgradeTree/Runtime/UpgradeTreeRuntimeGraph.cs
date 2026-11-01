using System.Collections.Generic;
using UnityEngine;

namespace Monoblok.UpgradeTree.Runtime
{
	public class UpgradeTreeRuntimeGraph : ScriptableObject
	{
		[SerializeReference]
		public List<UpgradeTreeRuntimeNode> Nodes = new List<UpgradeTreeRuntimeNode>();
	}
}
