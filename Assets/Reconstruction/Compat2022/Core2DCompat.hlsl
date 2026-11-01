#ifndef ENDLESS_RAILS_CORE2D_COMPAT_INCLUDED
#define ENDLESS_RAILS_CORE2D_COMPAT_INCLUDED

// The reconstructed sprite shaders were written against URP 17's Core2D.hlsl, which pulls in
// Core.hlsl and the sprite instancing helpers. URP 14 (Unity 2022.3) needs Core.hlsl first and
// names the per-sprite data differently.
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"

#ifdef UNITY_INSTANCING_ENABLED
    // URP 14 defines unity_SpriteColor and unity_SpriteFlip for instanced sprites.
    #define unity_SpriteProps float4(unity_SpriteFlip, 0, 0)
#else
    // Non-instanced 2022 sprites carry color in the vertex color and flip in the vertices.
    #ifndef unity_SpriteColor
        #define unity_SpriteColor float4(1, 1, 1, 1)
    #endif
    #define unity_SpriteProps float4(1, 1, 0, 0)
#endif

void SetUpSpriteInstanceProperties() {}

#endif
