using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace TsukimiNeko.AnimatableVolumeComponent
{
	[RequireComponent(typeof(Volume))]
	public class AnimatableVolumeHelper : MonoBehaviour
	{
		private Volume _cachedVolume;

		private Dictionary<Type, VolumeComponent> runtimeVolumeComponentDic = new Dictionary<Type, VolumeComponent>();

		private Volume volume
		{
			get
			{
				if (!_cachedVolume)
				{
					_cachedVolume = GetComponent<Volume>();
				}
				return _cachedVolume;
			}
		}

		public bool TryGet<T>(out T volumeComponent) where T : VolumeComponent
		{
			VolumeComponent value;
			bool result = runtimeVolumeComponentDic.TryGetValue(typeof(T), out value);
			volumeComponent = value as T;
			return result;
		}

		public bool TryGet(Type volumeComponentType, out VolumeComponent volumeComponent)
		{
			return runtimeVolumeComponentDic.TryGetValue(volumeComponentType, out volumeComponent);
		}

		private void Start()
		{
			if (Application.isPlaying)
			{
				ClearRuntimeProfile();
				CreateRuntimeProfile();
			}
		}

		public void CreateRuntimeProfile()
		{
			if (volume.HasInstantiatedProfile())
			{
				return;
			}
			VolumeProfile profile = volume.profile;
			runtimeVolumeComponentDic.Clear();
			foreach (VolumeComponent component in profile.components)
			{
				runtimeVolumeComponentDic[component.GetType()] = component;
			}
		}

		public void ClearRuntimeProfile()
		{
			if (volume.HasInstantiatedProfile())
			{
				VolumeProfile profile = volume.profile;
				volume.profile = null;
				UnityEngine.Object.DestroyImmediate(profile);
				runtimeVolumeComponentDic.Clear();
			}
		}

		public void AddCorrespondingComponent()
		{
			AnimatableVolumeComponentBase[] components = GetComponents<AnimatableVolumeComponentBase>();
			foreach (VolumeComponent component in volume.sharedProfile.components)
			{
				Type vcType = component.GetType();
				if (Array.FindIndex(components, (AnimatableVolumeComponentBase avc) => avc.TargetType == vcType) < 0 && AnimatableVolumeComponentMapping.Map.TryGetValue(vcType, out var value))
				{
					base.gameObject.AddComponent(value);
				}
			}
		}

		private void OnDestroy()
		{
			ClearRuntimeProfile();
		}
	}
}
