using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

namespace Monoblok.Train
{
	public class TrainConfigSelector : MonoBehaviour
	{
		[SerializeField]
		private TrainConfigPool pool;

		[SerializeField]
		private Train train;

		[SerializeField]
		private TrainConfigSelectorView view;

		[SerializeField]
		private Material defaultMaterial;

		[SerializeField]
		private Material lockedMaterial;

		[SerializeField]
		private EventReference selectRef;

		private bool hasInitialized;

		public event Action OnSelectionComplete;

		public event Action OnBackClicked;

		private void OnEnable()
		{
			view.OnConfigSelected += HandleConfigSelected;
			view.OnStartClicked += HandleStartClicked;
			view.OnBackClicked += HandleBackClicked;
		}

		private void OnDisable()
		{
			view.OnConfigSelected -= HandleConfigSelected;
			view.OnStartClicked -= HandleStartClicked;
			view.OnBackClicked -= HandleBackClicked;
		}

		private void HandleConfigSelected(TrainConfig config, bool isUnlocked)
		{
			train.Initialize(config);
			SpriteRenderer[] componentsInChildren = train.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = (isUnlocked ? defaultMaterial : lockedMaterial);
			}
			RuntimeManager.PlayOneShot(selectRef);
		}

		private void HandleStartClicked()
		{
			this.OnSelectionComplete?.Invoke();
		}

		private void HandleBackClicked()
		{
			this.OnBackClicked?.Invoke();
		}

		public void InitializeSelection()
		{
			IEnumerable<(TrainConfig, bool)> enumerable = pool.Entries.Select((TrainConfigEntry e) => (Config: e.Config, IsUnlocked: e.IsUnlocked));
			if (!hasInitialized)
			{
				view.Initialize(enumerable);
				hasInitialized = true;
			}
			else
			{
				view.RefreshPool(enumerable);
			}
			view.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			view.gameObject.SetActive(value: false);
		}
	}
}
