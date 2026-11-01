using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[ExecuteAlways]
	[RequireComponent(typeof(Volume))]
	[RequireComponent(typeof(AnimatableVolumeHelper))]
	public abstract class AnimatableVolumeComponentBase : MonoBehaviour
	{
		public bool active;

		private AnimatableVolumeHelper _cachedVolumeHelper;

		public abstract Type TargetType { get; }

		protected AnimatableVolumeHelper volumeHelper
		{
			get
			{
				if (!_cachedVolumeHelper)
				{
					_cachedVolumeHelper = GetComponent<AnimatableVolumeHelper>();
				}
				return _cachedVolumeHelper;
			}
		}

		protected virtual void LateUpdate()
		{
			WriteToVolumeComponent();
		}

		private void OnValidate()
		{
			volumeHelper.CreateRuntimeProfile();
			WriteToVolumeComponent();
			ReadFromVolumeComponent();
		}

		public abstract void WriteToVolumeComponent();

		public abstract void ReadFromVolumeComponent();
	}
}
