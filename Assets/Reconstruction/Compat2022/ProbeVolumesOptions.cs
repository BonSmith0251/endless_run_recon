using System;

namespace UnityEngine.Rendering
{
    // Core 14 (Unity 2022.3) has no ProbeVolumesOptions volume component for URP. This keeps the
    // URP 17 type name and serialized fields for the original default profile data and the game's
    // AnimatableProbeVolumesOptions wrapper. The game uses no Adaptive Probe Volumes, so nothing reads it.
    [Serializable]
    public sealed class APVLeakReductionModeParameter : VolumeParameter<APVLeakReductionMode>
    {
        public APVLeakReductionModeParameter(APVLeakReductionMode value, bool overrideState = false)
            : base(value, overrideState) { }
    }

    [Serializable]
    [VolumeComponentMenu("Lighting/Probe Volumes Options (URP 17 data, unused)")]
    public sealed class ProbeVolumesOptions : VolumeComponent
    {
        public ClampedFloatParameter normalBias = new ClampedFloatParameter(0.05f, 0f, 2f);
        public ClampedFloatParameter viewBias = new ClampedFloatParameter(0.1f, 0f, 2f);
        public BoolParameter scaleBiasWithMinProbeDistance = new BoolParameter(false);
        public ClampedFloatParameter samplingNoise = new ClampedFloatParameter(0.1f, 0f, 0.5f);
        public BoolParameter animateSamplingNoise = new BoolParameter(true);
        public APVLeakReductionModeParameter leakReductionMode = new APVLeakReductionModeParameter(APVLeakReductionMode.ValidityAndNormalBased);
        public ClampedFloatParameter minValidDotProductValue = new ClampedFloatParameter(0.1f, -1f, 0.33f);
        public BoolParameter occlusionOnlyReflectionNormalization = new BoolParameter(true);
        public ClampedFloatParameter intensityMultiplier = new ClampedFloatParameter(1f, 0f, 1f);
        public ClampedFloatParameter skyOcclusionIntensityMultiplier = new ClampedFloatParameter(1f, 0f, 5f);
        public Vector3Parameter worldOffset = new Vector3Parameter(Vector3.zero);
    }
}
