using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Monoblok.SA;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace Monoblok
{
	public class Bootstrap : MonoBehaviour
	{
		[BankRef]
		[SerializeField]
		private List<string> banks = new List<string>();

		[SerializeField]
		[Scene]
		private string gameScene;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			SceneManager.LoadScene("Bootstrap", LoadSceneMode.Single);
			Debug.Log("Loading Bootstrap Scene");
		}

		private void Start()
		{
			Debug.Log("Bootstrap scene loaded");
			PrimeTweenConfig.warnZeroDuration = false;
			PrimeTweenConfig.warnEndValueEqualsCurrent = false;
			PrimeTweenConfig.warnTweenOnDisabledTarget = false;
			PrimeTweenConfig.SetTweensCapacity(10000);
			ScriptableObject[] array = Resources.LoadAll<ScriptableObject>("Resettables");
			foreach (ScriptableObject scriptableObject in array)
			{
				if (!(scriptableObject == null) && scriptableObject is IResettable resettable)
				{
					resettable.ApplyReset();
				}
			}
			Debug.Log("Resettables reset");
			StartCoroutine(LoadGameAsync());
		}

		private IEnumerator LoadGameAsync()
		{
			Debug.Log("Loading game async...");
			// Unity coroutines do not await a System.Threading.Tasks.Task. Yield the
			// Addressables operation itself so warm FMOD loads cannot race localization.
			var localization = LocalizationSettings.InitializationOperation;
			yield return localization;
			if (localization.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
			{
				Debug.LogError("Localization initialization failed: " + localization.OperationException);
				yield break;
			}
			Debug.Log("Localization initialized");
			foreach (string bank in banks)
			{
				RuntimeManager.LoadBank(bank, loadSamples: true);
			}
			while (!RuntimeManager.HaveAllBanksLoaded)
			{
				yield return null;
			}
			Debug.Log("FMOD banks loaded");
			while (RuntimeManager.AnySampleDataLoading())
			{
				yield return null;
			}
			Debug.Log("FMOD sample data loaded");
			AsyncOperation async = SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Single);
			Debug.Log("Loading game scene async...");
			while (!async.isDone)
			{
				yield return null;
			}
			Debug.Log("Game load finished");
		}
	}
}
