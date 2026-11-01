Shader "Shader Graphs/DiagonalGradientShader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		_Color ("Color", Color) = (0,0.1720123,1,1)
		_Color_1 ("Color (1)", Color) = (1,0,0,1)
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		_Offset ("Offset", Float) = 0
		[HideInInspector] White ("Color", Color) = (1,1,1,1)
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	// Evidence-based reconstruction: exact DXBC fragment instructions and original parameter bindings.
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
        float4 _Color;
        float4 _Color_1;
        float _Offset;
        CBUFFER_END
        Texture2D<float4> _MainTex; SamplerState sampler_MainTex;
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0;
            float4 cb0[5];
            cb0[0]=0;
            cb0[1]=0;
            cb0[2]=0;
            cb0[3]=0;
            cb0[4]=0;
            float4 cb1[4];
            cb1[0]=0;
            cb1[1]=0;
            cb1[2]=0;
            cb1[3]=0;
            cb0[4].xy=_GlobalMipBias;
            cb1[0].xyzw=_Color;
            cb1[1].xyzw=_Color_1;
            cb1[3].x=_Offset;
            r0.xyzw=(asuint((_MainTex.SampleBias(sampler_MainTex,(v1.xyxx).xy,(cb0[4].xxxx).x)).xyzw)).xyzw;
            r1.x=(asuint(asfloat(uint4((asfloat(r0.wwww))==(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).x;
            if(((r1.xxxx).x!=0u)) discard;
            r1.x=(asuint((-(v1.xxxx))+(v1.yyyy))).x;
            r1.x=(asuint(mad(asfloat(r1.xxxx),asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u)),v1.xxxx))).x;
            r1.x=(asuint((asfloat(r1.xxxx))+(cb1[3].xxxx))).x;
            r1.yzw=(asuint((cb1[0].xxyz)+(-(cb1[1].xxyz)))).yzw;
            r1.xyz=(asuint(mad(asfloat(r1.xxxx),asfloat(r1.yzwy),cb1[1].xyzx))).xyz;
            r0.xyz=(asuint((asfloat(r0.xyzx))*(asfloat(r1.xyzx)))).xyz;
            o0.xyzw=((asfloat(r0.xyzw))*(v2.xyzw)).xyzw;
            return o0;
        }
        ENDHLSL
        Pass {
            Name "Sprite Unlit"
            Tags { "LightMode"="Universal2D" }
            HLSLPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
        Pass {
            Name "Sprite Unlit"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
}
