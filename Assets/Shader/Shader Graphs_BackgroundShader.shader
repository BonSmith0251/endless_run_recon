Shader "Shader Graphs/BackgroundShader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		_BaseColor ("BaseColor", Color) = (0.7450981,0.5372549,0.4588236,1)
		_Color_Change ("Color Change", Range(0, 1)) = 0
		_NextBaseColor ("NextBaseColor", Color) = (0.4941176,0.6039216,0.7490196,1)
		_Edge1 ("Edge1", Float) = 0
		_Edge2 ("Edge2", Float) = 1
		_ColorDiff ("ColorDiff", Float) = -0.21
		_OverlayOffset ("OverlayOffset", Float) = 0
		_OverlayTiling ("OverlayTiling", Vector) = (1,1,0,0)
		_WorldTiling ("WorldTiling", Vector) = (1,1,0,0)
		_WorldOffset ("WorldOffset", Vector) = (0,0,0,0)
		_Rot1 ("Rot1", Float) = 0
		_Rot2 ("Rot2", Float) = 0
		[NoScaleOffset] _SampleTexture2D_ba46ea2a9c7748839cc33f9ebfedff02_Texture_1_Texture2D ("Texture2D", 2D) = "white" {}
		[HideInInspector] White ("Color", Color) = (1,1,1,1)
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	// Evidence-based reconstruction from original D3D11 DXBC, not original graph source.
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" "UniversalMaterialType"="Unlit" }
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off ZWrite Off ZTest LEqual

        HLSLINCLUDE
        #include "Assets/Reconstruction/Compat2022/Core2DCompat.hlsl"
        float4 _RendererColor;
        struct Attributes { float3 positionOS:POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; UNITY_VERTEX_INPUT_INSTANCE_ID };
        struct Varyings { float4 positionCS:SV_POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; float3 positionWS:TEXCOORD1; UNITY_VERTEX_OUTPUT_STEREO };
        Varyings vert(Attributes v) {
            Varyings o=(Varyings)0;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            SetUpSpriteInstanceProperties();
            v.positionOS=UnityFlipSprite(v.positionOS,unity_SpriteProps.xy);
            o.positionWS=TransformObjectToWorld(v.positionOS);
            o.positionCS=TransformWorldToHClip(o.positionWS);
            o.uv=v.uv;
            o.color=v.color*_RendererColor*unity_SpriteColor;
            return o;
        }
        CBUFFER_START(UnityPerMaterial)
        float4 _BaseColor,_NextBaseColor;
        float _Color_Change,_Edge1,_Edge2,_ColorDiff,_OverlayOffset;
        float2 _OverlayTiling,_WorldTiling,_WorldOffset;
        float _Rot1,_Rot2;
        CBUFFER_END
        TEXTURE2D(_SampleTexture2D_ba46ea2a9c7748839cc33f9ebfedff02_Texture_1_Texture2D);
        SAMPLER(sampler_SampleTexture2D_ba46ea2a9c7748839cc33f9ebfedff02_Texture_1_Texture2D);
        float4 frag(Varyings i):SV_Target {
            float2 p=i.positionWS.xy*_WorldTiling+_WorldOffset-0.5;
            float s2,c2; sincos(radians(_Rot2),s2,c2);
            float2 q=float2(dot(p,float2(c2,s2)),dot(p,float2(-s2,c2)))+0.5;
            q=q*_OverlayTiling+float2(_OverlayOffset,0);
            float3 overlay=SAMPLE_TEXTURE2D_BIAS(_SampleTexture2D_ba46ea2a9c7748839cc33f9ebfedff02_Texture_1_Texture2D,sampler_SampleTexture2D_ba46ea2a9c7748839cc33f9ebfedff02_Texture_1_Texture2D,q,_GlobalMipBias.x).rgb;
            float s1,c1; sincos(radians(_Rot1),s1,c1);
            float axis=dot(p,float2(c1,s1));
            float edge=saturate(axis)+saturate(-axis);
            float t=saturate((edge-_Edge1)/(_Edge2-_Edge1));
            t=min(t*t*(3-2*t),1.0);
            float diagonal=dot(i.uv-0.5,float2(-0.70710678,0.70710678))+0.5;
            float change=saturate((4*_Color_Change-1-diagonal)*1.25);
            change=change*change*(3-2*change);
            float3 base=lerp(_BaseColor.rgb,_NextBaseColor.rgb,change);
            return float4(saturate(base+t*overlay*_ColorDiff),1)*i.color;
        }
        ENDHLSL
        Pass {
            Name "Sprite Unlit"
            Tags { "LightMode"="Universal2D" }
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
        Pass {
            Name "Sprite Unlit"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
}
