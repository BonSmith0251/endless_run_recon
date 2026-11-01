namespace UnityEngine.Rendering.Universal
{
    // URP 17's Bloom filter choice. URP 14 Bloom has only the Gaussian filter, which is what
    // both of the game's Bloom profiles select (value 0), so AnimatableBloom keeps the value
    // without passing it to the volume component.
    public enum BloomFilterMode
    {
        Gaussian = 0,
        Dual = 1,
        Kawase = 2
    }
}
