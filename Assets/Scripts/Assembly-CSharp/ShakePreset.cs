using System;

[Serializable]
public struct ShakePreset
{
	public float amplitude;

	public float frequency;

	public float duration;

	public static ShakePreset Light => new ShakePreset(0.5f, 2f, 0.15f);

	public static ShakePreset Medium => new ShakePreset(1.5f, 2.5f, 0.3f);

	public static ShakePreset Heavy => new ShakePreset(3f, 3f, 0.5f);

	public static ShakePreset Jolt => new ShakePreset(2f, 4f, 0.1f);

	public ShakePreset(float amplitude, float frequency, float duration)
	{
		this.amplitude = amplitude;
		this.frequency = frequency;
		this.duration = duration;
	}
}
