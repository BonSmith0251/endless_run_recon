using System;
using Monoblok.StatSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace Monoblok.Train
{
	[Serializable]
	public class CartBufferRule : ICartBufferRule
	{
		private static readonly LocalizedString descriptionTemplate = new LocalizedString("Modules", "buffer_line");

		[SerializeReference]
		[SubclassSelector]
		private ITrainCartTargetProvider targetProvider;

		[SerializeField]
		private StatDefinition statDefinition;

		[SerializeField]
		private StatModifierFactory modifierFactory;

		[SerializeField]
		private float maxValue = float.MaxValue;

		[SerializeField]
		private bool buffOrDebuff = true;

		public ITrainCartTargetProvider TargetProvider => targetProvider;

		public StatDefinition StatDefinition => statDefinition;

		public StatModifierFactory ModifierFactory => modifierFactory;

		public float MaxValue => maxValue;

		public bool BuffOrDebuff => buffOrDebuff;

		public string GetDescription()
		{
			return descriptionTemplate.GetLocalizedString(new
			{
				value = modifierFactory.GetColoredString(),
				stat = statDefinition.Name,
				target = targetProvider.GetTargetPhrase()
			});
		}
	}
}
