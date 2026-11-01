Shader "Shader Graphs/GroundDetailsShader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		_OffsetUvX ("OffsetUvX", Float) = 0
		_OffsetUvY ("OffsetUvY", Float) = 0
		_tiling ("tiling", Float) = 1
		_BlurAmount ("BlurAmount", Float) = 0.001
		_Tiling ("Tiling", Float) = 1
		_TilingVector ("TilingVector", Vector) = (0.01,0.02,0,0)
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
        float _OffsetUvX,_OffsetUvY,_BlurAmount;
        float2 _TilingVector;
        CBUFFER_END
        TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
        float4 sampleGround(float2 uv) { return SAMPLE_TEXTURE2D_BIAS(_MainTex,sampler_MainTex,uv,_GlobalMipBias.x); }
        float4 frag(Varyings i):SV_Target {
            float2 uv=i.positionWS.xy*_TilingVector+float2(_OffsetUvX,_OffsetUvY);
            float4 c=sampleGround(uv+float2(_BlurAmount,0))+sampleGround(uv-float2(_BlurAmount,0));
            c=sampleGround(uv)+c;
            c=sampleGround(uv+float2(0,_BlurAmount))+c;
            c=sampleGround(uv-float2(0,_BlurAmount))+c;
            if(c.a==0) discard;
            return c*0.2*i.color;
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
