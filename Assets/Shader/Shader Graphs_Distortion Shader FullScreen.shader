Shader "Shader Graphs/Distortion Shader FullScreen" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		_Strength ("Strength", Range(-5, 5)) = -0.1
		_WaveDistanceFromCenter ("WaveDistanceFromCenter", Range(-0.1, 1)) = 0.5
		_Size ("Size", Float) = 0
		_Center ("Center", Vector) = (0.5,0.5,0,0)
		_Radius ("Radius", Range(0, 1)) = 1
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
        float _Strength;
        float _WaveDistanceFromCenter;
        float _Size;
        float2 _Center;
        float _Radius;
        CBUFFER_END
        Texture2D<float4> _CameraSortingLayerTexture; SamplerState sampler_CameraSortingLayerTexture;
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0;
            float4 cb0[23];
            cb0[0]=0;
            cb0[1]=0;
            cb0[2]=0;
            cb0[3]=0;
            cb0[4]=0;
            cb0[5]=0;
            cb0[6]=0;
            cb0[7]=0;
            cb0[8]=0;
            cb0[9]=0;
            cb0[10]=0;
            cb0[11]=0;
            cb0[12]=0;
            cb0[13]=0;
            cb0[14]=0;
            cb0[15]=0;
            cb0[16]=0;
            cb0[17]=0;
            cb0[18]=0;
            cb0[19]=0;
            cb0[20]=0;
            cb0[21]=0;
            cb0[22]=0;
            float4 cb1[3];
            cb1[0]=0;
            cb1[1]=0;
            cb1[2]=0;
            cb0[3].xyzw=_ScaledScreenParams;
            cb0[4].xy=_GlobalMipBias;
            cb0[22].xyzw=_ProjectionParams;
            cb1[1].x=_Strength;
            cb1[1].y=_WaveDistanceFromCenter;
            cb1[1].z=_Size;
            cb1[2].xy=_Center;
            cb1[2].z=_Radius;
            r0.xy=(asuint((cb1[1].yzyy)*(cb1[2].zzzz))).xy;
            r0.x=(asuint((asfloat(r0.yyyy))+(asfloat(r0.xxxx)))).x;
            r0.y=(asuint(mad(cb1[1].yyyy,cb1[2].zzzz,-(asfloat(r0.yyyy))))).y;
            r0.x=(asuint((-(asfloat(r0.yyyy)))+(asfloat(r0.xxxx)))).x;
            r0.x=(asuint((asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))/(asfloat(r0.xxxx)))).x;
            r0.zw=(asuint((v1.xxxy)+(-(cb1[2].xxxy)))).zw;
            r1.x=(asuint(float4(dot((asfloat(r0.zwzz)).xy,(asfloat(r0.zwzz)).xy),dot((asfloat(r0.zwzz)).xy,(asfloat(r0.zwzz)).xy),dot((asfloat(r0.zwzz)).xy,(asfloat(r0.zwzz)).xy),dot((asfloat(r0.zwzz)).xy,(asfloat(r0.zwzz)).xy)))).x;
            r1.y=(asuint(sqrt(asfloat(r1.xxxx)))).y;
            r1.x=(asuint(rsqrt(asfloat(r1.xxxx)))).x;
            r0.zw=(asuint((asfloat(r0.zzzw))*(asfloat(r1.xxxx)))).zw;
            r0.zw=(asuint((asfloat(r0.zzzw))*(cb1[1].xxxx))).zw;
            r0.y=(asuint((-(asfloat(r0.yyyy)))+(asfloat(r1.yyyy)))).y;
            r0.x=(asuint(saturate((asfloat(r0.xxxx))*(asfloat(r0.yyyy))))).x;
            r0.y=(asuint(mad(asfloat(r0.xxxx),asfloat(uint4(0xc0000000u,0xc0000000u,0xc0000000u,0xc0000000u)),asfloat(uint4(0x40400000u,0x40400000u,0x40400000u,0x40400000u))))).y;
            r0.x=(asuint((asfloat(r0.xxxx))*(asfloat(r0.xxxx)))).x;
            r1.x=(asuint((asfloat(r0.xxxx))*(asfloat(r0.yyyy)))).x;
            r0.x=(asuint(mad(-(asfloat(r0.yyyy)),asfloat(r0.xxxx),asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u))))).x;
            r0.x=(asuint((asfloat(r1.xxxx))*(asfloat(r0.xxxx)))).x;
            r0.xy=(asuint((asfloat(r0.zwzz))*(asfloat(r0.xxxx)))).xy;
            r0.z=(asuint(asfloat(uint4((cb0[22].xxxx)<(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).z;
            r0.w=(asuint((-(v0.yyyy))+(cb0[3].yyyy))).w;
            r1.y=((r0.zzzz!=0u)?(r0.wwww):(asuint(v0.yyyy))).y;
            r1.x=(asuint(v0.xxxx)).x;
            r1.xy=(asuint((asfloat(r1.xyxx))/(cb0[3].xyxx))).xy;
            r1.z=(asuint((-(asfloat(r1.yyyy)))+(asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u))))).z;
            r0.z=(asuint(saturate((-(cb1[1].yyyy))+(asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))))).z;
            r0.xy=(asuint(mad(asfloat(r0.zzzz),asfloat(r0.xyxx),asfloat(r1.xzxx)))).xy;
            r0.xyzw=(asuint((_CameraSortingLayerTexture.SampleBias(sampler_CameraSortingLayerTexture,(asfloat(r0.xyxx)).xy,(cb0[4].xxxx).x)).xyzw)).xyzw;
            r0.w=(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)).w;
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
