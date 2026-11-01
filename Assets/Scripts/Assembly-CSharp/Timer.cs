using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
	[SerializeField]
	private float timerMin = 3f;

	[SerializeField]
	private float timerMax = 10f;

	private float timer = 10f;

	[SerializeField]
	private bool randomStart;

	public UnityEvent<float> timerEvent;

	private float _time;

	public float TheTime
	{
		get
		{
			return _time;
		}
		set
		{
			_time = value;
			if (_time > 1f)
			{
				_time = 0f;
			}
			timerEvent?.Invoke(_time);
		}
	}

	private void Start()
	{
		timer = Random.Range(timerMin, timerMax);
		if (randomStart)
		{
			TheTime = Random.Range(0f, 1f);
		}
	}

	private void Update()
	{
		TheTime += Time.deltaTime / timer;
	}
}
