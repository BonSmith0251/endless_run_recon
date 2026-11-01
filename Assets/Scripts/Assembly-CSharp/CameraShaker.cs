using System.Collections;
using NaughtyAttributes;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	[Header("References")]
	[Tooltip("The CinemachineCamera (virtual camera) to shake.")]
	[SerializeField]
	private CinemachineCamera virtualCamera;

	[Header("Default Shake Preset")]
	[Tooltip("Default peak amplitude when no override is supplied.")]
	[SerializeField]
	private float defaultAmplitude = 1.5f;

	[Tooltip("Default noise frequency when no override is supplied.")]
	[SerializeField]
	private float defaultFrequency = 2f;

	[Tooltip("Default shake duration in seconds.")]
	[SerializeField]
	private float defaultDuration = 0.3f;

	private CinemachineBasicMultiChannelPerlin _perlin;

	private Coroutine _shakeCoroutine;

	public static CameraShaker Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		if (virtualCamera == null)
		{
			Debug.LogError("[CameraShaker] No CinemachineCamera assigned.", this);
			return;
		}
		_perlin = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
		if (_perlin == null)
		{
			Debug.LogError("[CameraShaker] CinemachineBasicMultiChannelPerlin not found on the virtual camera. Add it via Add Component → Cinemachine → Basic Multi Channel Perlin.", this);
			return;
		}
		_perlin.AmplitudeGain = 0f;
		_perlin.FrequencyGain = defaultFrequency;
	}

	[Button(null, EButtonEnableMode.Always)]
	public static void Shake()
	{
		if (Instance == null)
		{
			Debug.LogWarning("[CameraShaker] No CameraShaker instance found in the scene.");
		}
		else
		{
			Instance.DoShake(Instance.defaultAmplitude, Instance.defaultFrequency, Instance.defaultDuration);
		}
	}

	public static void Shake(float amplitude, float frequency, float duration)
	{
		if (Instance == null)
		{
			Debug.LogWarning("[CameraShaker] No CameraShaker instance found in the scene.");
		}
		else
		{
			Instance.DoShake(amplitude, frequency, duration);
		}
	}

	public static void Shake(ShakePreset preset)
	{
		Shake(preset.amplitude, preset.frequency, preset.duration);
	}

	private void DoShake(float amplitude, float frequency, float duration)
	{
		if (!(_perlin == null))
		{
			if (_shakeCoroutine != null)
			{
				StopCoroutine(_shakeCoroutine);
			}
			_shakeCoroutine = StartCoroutine(ShakeRoutine(amplitude, frequency, duration));
		}
	}

	private IEnumerator ShakeRoutine(float amplitude, float frequency, float duration)
	{
		_perlin.FrequencyGain = frequency;
		_perlin.AmplitudeGain = amplitude;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			_perlin.AmplitudeGain = Mathf.Lerp(amplitude, 0f, elapsed / duration);
			yield return null;
		}
		_perlin.AmplitudeGain = 0f;
		_shakeCoroutine = null;
	}
}
