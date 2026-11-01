using System;
using PrimeTween;
using UnityEngine;

namespace Monoblok.Train
{
	public class ModuleSlot
	{
		public readonly int Index;

		public readonly TrainCart Cart;

		public readonly Vector2 LocalPosition;

		private ModuleData data;

		private ModuleBehavior behavior;

		private bool isRemoving;

		public ModuleData ModuleData => data;

		public ModuleBehavior Behavior => behavior;

		public bool IsFree => data == null;

		public event Action<ModuleData> OnModuleAdded;

		public event Action OnModuleRemoved;

		public ModuleSlot(TrainCart cart, Vector2 localPosition, int index)
		{
			Cart = cart;
			LocalPosition = localPosition;
			Index = index;
		}

		public void AddModule(ModuleData moduleData, bool animate = true, bool showVisual = true)
		{
			RemoveModule();
			data = moduleData;
			behavior = moduleData.Definition.CreateBehavior(Cart.transform);
			behavior.SpriteRenderer.enabled = showVisual;
			behavior.transform.SetParent(Cart.SpriteRenderer.transform);
			behavior.Attach(this);
			this.OnModuleAdded?.Invoke(moduleData);
			if (animate)
			{
				behavior.transform.localScale = Vector2.zero;
				Vector3 localPosition = behavior.transform.localPosition;
				behavior.transform.localPosition += new Vector3(0f, 1f, 0f);
				Sequence.Create().Chain(Tween.Scale(behavior.transform, 1f, 0.15f, Ease.OutBack)).ChainDelay(0.15f)
					.Chain(Tween.LocalPosition(behavior.transform, localPosition, 0.15f));
			}
		}

		public void RemoveModule()
		{
			if (data != null && !isRemoving)
			{
				isRemoving = true;
				behavior.Detach();
				UnityEngine.Object.Destroy(behavior.gameObject);
				behavior = null;
				data = null;
				isRemoving = false;
				this.OnModuleRemoved?.Invoke();
			}
		}
	}
}
