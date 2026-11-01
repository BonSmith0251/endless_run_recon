using System;

namespace UnityEngine.Rendering.Universal
{
    // URP 14 (Unity 2022.3) has no Screen Space Lens Flare. This keeps the URP 17 volume
    // component's type name and serialized fields so the original profile data and the game's
    // AnimatableScreenSpaceLensFlare keep working. The effect itself is not rendered; the
    // shipped DefaultVolumeProfile has intensity 0, which disables it in the original too.
    public enum ScreenSpaceLensFlareResolution
    {
        Full = 1,
        Half = 2,
        Quarter = 4,
        Eighth = 8
    }

    [Serializable]
    public sealed class ScreenSpaceLensFlareResolutionParameter : VolumeParameter<ScreenSpaceLensFlareResolution>
    {
        public ScreenSpaceLensFlareResolutionParameter(ScreenSpaceLensFlareResolution value, bool overrideState = false)
            : base(value, overrideState) { }
    }

    [Serializable]
    [VolumeComponentMenu("Post-processing/Screen Space Lens Flare (URP 17 data, not rendered)")]
    public sealed class ScreenSpaceLensFlare : VolumeComponent, IPostProcessComponent
    {
        public MinFloatParameter intensity = new MinFloatParameter(0f, 0f);
        public ColorParameter tintColor = new ColorParameter(Color.white);
        public ClampedIntParameter bloomMip = new ClampedIntParameter(1, 0, 5);
        public MinFloatParameter firstFlareIntensity = new MinFloatParameter(1f, 0f);
        public MinFloatParameter secondaryFlareIntensity = new MinFloatParameter(1f, 0f);
        public MinFloatParameter warpedFlareIntensity = new MinFloatParameter(1f, 0f);
        public Vector2Parameter warpedFlareScale = new Vector2Parameter(new Vector2(1f, 1f));
        public ClampedIntParameter samples = new ClampedIntParameter(1, 1, 3);
        public ClampedFloatParameter sampleDimmer = new ClampedFloatParameter(0.5f, 0.1f, 1f);
        public ClampedFloatParameter vignetteEffect = new ClampedFloatParameter(1f, 0f, 1f);
        public ClampedFloatParameter startingPosition = new ClampedFloatParameter(1.25f, 1f, 3f);
        public ClampedFloatParameter scale = new ClampedFloatParameter(1.5f, 1f, 4f);
        public MinFloatParameter streaksIntensity = new MinFloatParameter(0f, 0f);
        public ClampedFloatParameter streaksLength = new ClampedFloatParameter(0.5f, 0f, 1f);
        public FloatParameter streaksOrientation = new FloatParameter(0f);
        public ClampedFloatParameter streaksThreshold = new ClampedFloatParameter(0.25f, 0f, 1f);
        public ScreenSpaceLensFlareResolutionParameter resolution = new ScreenSpaceLensFlareResolutionParameter(ScreenSpaceLensFlareResolution.Quarter);
        public ClampedFloatParameter chromaticAbberationIntensity = new ClampedFloatParameter(0.5f, 0f, 1f);

        public bool IsActive() => intensity.value > 0f;

        public bool IsTileCompatible() => false;
    }
}
