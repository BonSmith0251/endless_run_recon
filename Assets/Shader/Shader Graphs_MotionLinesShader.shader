Shader "Shader Graphs/MotionLinesShader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		_Noise_Scale ("Noise Scale", Float) = 100
		_Noise_TIling ("Noise TIling", Vector) = (0.01,1,0,0)
		_Rotation ("Rotation", Float) = -30
		_StepEdge ("StepEdge", Float) = 0.6
		_Transparency ("Transparency", Range(0, 1)) = 1
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
        float _Noise_Scale;
        float2 _Noise_TIling;
        float _Rotation;
        float _StepEdge;
        float _Transparency;
        float _Offset;
        CBUFFER_END
        Texture2D<float4> _MainTex; SamplerState sampler_MainTex;
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0,r2=0,r3=0,r4=0,r5=0;
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
            cb1[0].x=_Noise_Scale;
            cb1[0].yz=_Noise_TIling;
            cb1[0].w=_Rotation;
            cb1[1].x=_StepEdge;
            cb1[1].y=_Transparency;
            cb1[3].x=_Offset;
            r0.xyz=(asuint((cb1[0].wxxw)*(asfloat(uint4(0x3c8efa35u,0x3f000000u,0x3e800000u,0x00000000u))))).xyz;
            r1.xy=(asuint((v3.xyxx)+(asfloat(uint4(0xbf000000u,0xbf000000u,0x00000000u,0x00000000u))))).xy;
            float4 trig0=asfloat(r0.xxxx);
            r0.x=(asuint(sin(trig0))).x;
            r2.x=(asuint(cos(trig0))).x;
            r3.x=(asuint(-(asfloat(r0.xxxx)))).x;
            r3.y=(r2.xxxx).y;
            r2.y=(asuint(float4(dot((asfloat(r1.xyxx)).xy,(asfloat(r3.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r3.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r3.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r3.xyxx)).xy)))).y;
            r3.z=(r0.xxxx).z;
            r2.x=(asuint(float4(dot((asfloat(r1.xyxx)).xy,(asfloat(r3.yzyy)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r3.yzyy)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r3.yzyy)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r3.yzyy)).xy)))).x;
            r0.xw=(asuint((asfloat(r2.xxxy))+(asfloat(uint4(0x3f000000u,0x00000000u,0x00000000u,0x3f000000u))))).xw;
            r1.w=(asuint((asfloat(r0.wwww))*(cb1[0].zzzz))).w;
            r1.xz=(asuint(mad(asfloat(r0.xxxx),cb1[0].yyyy,cb1[3].xxxx))).xz;
            r1.yw=(r1.wwww).yw;
            r0.xw=(asuint((asfloat(r1.zzzw))*(cb1[0].xxxx))).xw;
            r2.xy=(asuint(floor(asfloat(r0.xwxx)))).xy;
            r0.xw=(asuint(frac(asfloat(r0.xxxw)))).xw;
            r2.zw=(asuint((asfloat(r0.xxxw))*(asfloat(r0.xxxw)))).zw;
            r0.xw=(asuint(mad(-(asfloat(r0.xxxw)),asfloat(uint4(0x40000000u,0x00000000u,0x00000000u,0x40000000u)),asfloat(uint4(0x40400000u,0x00000000u,0x00000000u,0x40400000u))))).xw;
            r0.xw=(asuint((asfloat(r0.xxxw))*(asfloat(r2.zzzw)))).xw;
            r3.xyzw=(asuint((asfloat(r2.xyxy))+(asfloat(uint4(0x3f800000u,0x00000000u,0x00000000u,0x3f800000u))))).xyzw;
            r2.zw=(asuint((asfloat(r2.xxxy))+(asfloat(uint4(0x00000000u,0x00000000u,0x3f800000u,0x3f800000u))))).zw;
            r2.xyzw=(asuint(int4(asfloat(r2.xyzw)))).xyzw;
            r2.yw=((r2.yyyw)^(uint4(0x00000000u,0x41c64e6du,0x00000000u,0x41c64e6du))).yw;
            r2.xz=((r2.yywy)+(r2.xxzx)).xz;
            r2.xz=((r2.yywy)*(r2.xxzx)).xz;
            r2.y=((r2.xxxx)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).y;
            r2.x=((r2.yyyy)^(r2.xxxx)).x;
            r2.x=((r2.xxxx)*(uint4(0x27d4eb2du,0x27d4eb2du,0x27d4eb2du,0x27d4eb2du))).x;
            r2.x=((r2.xxxx)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).x;
            r2.x=(asuint(float4(r2.xxxx))).x;
            r2.x=(asuint((asfloat(r2.xxxx))*(asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u))))).x;
            r3.xyzw=(asuint(int4(asfloat(r3.xyzw)))).xyzw;
            r3.yw=((r3.yyyw)^(uint4(0x00000000u,0x41c64e6du,0x00000000u,0x41c64e6du))).yw;
            r3.xz=((r3.yywy)+(r3.xxzx)).xz;
            r3.xy=((r3.ywyy)*(r3.xzxx)).xy;
            r3.zw=((r3.xxxy)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).zw;
            r3.xy=((r3.zwzz)^(r3.xyxx)).xy;
            r3.xy=((r3.xyxx)*(uint4(0x27d4eb2du,0x27d4eb2du,0x00000000u,0x00000000u))).xy;
            r3.xy=((r3.xyxx)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).xy;
            r3.xy=(asuint(float4(r3.xyxx))).xy;
            r2.y=(asuint((asfloat(r3.yyyy))*(asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u))))).y;
            r2.w=((r2.zzzz)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).w;
            r2.z=((r2.wwww)^(r2.zzzz)).z;
            r2.z=((r2.zzzz)*(uint4(0x27d4eb2du,0x27d4eb2du,0x27d4eb2du,0x27d4eb2du))).z;
            r2.z=((r2.zzzz)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).z;
            r2.z=(asuint(float4(r2.zzzz))).z;
            r2.w=(asuint(mad(asfloat(r3.xxxx),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r2.xxxx))))).w;
            r2.x=(asuint(mad(asfloat(r0.xxxx),asfloat(r2.wwww),asfloat(r2.xxxx)))).x;
            r2.z=(asuint(mad(asfloat(r2.zzzz),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r2.yyyy))))).z;
            r0.x=(asuint(mad(asfloat(r0.xxxx),asfloat(r2.zzzz),asfloat(r2.yyyy)))).x;
            r0.x=(asuint((-(asfloat(r2.xxxx)))+(asfloat(r0.xxxx)))).x;
            r0.x=(asuint(mad(asfloat(r0.wwww),asfloat(r0.xxxx),asfloat(r2.xxxx)))).x;
            r1.xyzw=(asuint((asfloat(r0.yyzz))*(asfloat(r1.xyzw)))).xyzw;
            r2.xyzw=(asuint(floor(asfloat(r1.xyzw)))).xyzw;
            r1.xyzw=(asuint(frac(asfloat(r1.xyzw)))).xyzw;
            r3.xyzw=(asuint((asfloat(r1.xyzw))*(asfloat(r1.xyzw)))).xyzw;
            r1.xyzw=(asuint(mad(-(asfloat(r1.xyzw)),asfloat(uint4(0x40000000u,0x40000000u,0x40000000u,0x40000000u)),asfloat(uint4(0x40400000u,0x40400000u,0x40400000u,0x40400000u))))).xyzw;
            r1.xyzw=(asuint((asfloat(r1.xyzw))*(asfloat(r3.xyzw)))).xyzw;
            r3.xyzw=(asuint((asfloat(r2.xyxy))+(asfloat(uint4(0x3f800000u,0x00000000u,0x00000000u,0x3f800000u))))).xyzw;
            r4.xyzw=(asuint((asfloat(r2.xyzw))+(asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x00000000u))))).xyzw;
            r5.xyzw=(asuint(int4(asfloat(r2.xyzw)))).xyzw;
            r0.yz=((r5.yywy)^(uint4(0x00000000u,0x41c64e6du,0x41c64e6du,0x00000000u))).yz;
            r2.xy=((r0.yzyy)+(r5.xzxx)).xy;
            r0.yz=((r0.yyzy)*(r2.xxyx)).yz;
            r2.xy=((r0.yzyy)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).xy;
            r0.yz=((r0.yyzy)^(r2.xxyx)).yz;
            r0.yz=((r0.yyzy)*(uint4(0x00000000u,0x27d4eb2du,0x27d4eb2du,0x00000000u))).yz;
            r0.yz=((r0.yyzy)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).yz;
            r0.yz=(asuint(float4(r0.yyzy))).yz;
            r0.yz=(asuint((asfloat(r0.yyzy))*(asfloat(uint4(0x00000000u,0x33800001u,0x33800001u,0x00000000u))))).yz;
            r3.xyzw=(asuint(int4(asfloat(r3.xyzw)))).xyzw;
            r2.xy=((r3.ywyy)^(uint4(0x41c64e6du,0x41c64e6du,0x00000000u,0x00000000u))).xy;
            r3.xy=((r2.xyxx)+(r3.xzxx)).xy;
            r2.xy=((r2.xyxx)*(r3.xyxx)).xy;
            r3.xy=((r2.xyxx)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).xy;
            r2.xy=((r2.xyxx)^(r3.xyxx)).xy;
            r2.xy=((r2.xyxx)*(uint4(0x27d4eb2du,0x27d4eb2du,0x00000000u,0x00000000u))).xy;
            r2.xy=((r2.xyxx)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).xy;
            r2.xy=(asuint(float4(r2.xyxx))).xy;
            r0.w=(asuint((asfloat(r2.yyyy))*(asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u))))).w;
            r3.xyzw=(asuint(int4(asfloat(r4.xyzw)))).xyzw;
            r3.yw=((r3.yyyw)^(uint4(0x00000000u,0x41c64e6du,0x00000000u,0x41c64e6du))).yw;
            r3.xz=((r3.yywy)+(r3.xxzx)).xz;
            r3.xy=((r3.ywyy)*(r3.xzxx)).xy;
            r3.zw=((r3.xxxy)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).zw;
            r3.xy=((r3.zwzz)^(r3.xyxx)).xy;
            r3.xy=((r3.xyxx)*(uint4(0x27d4eb2du,0x27d4eb2du,0x00000000u,0x00000000u))).xy;
            r3.xy=((r3.xyxx)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).xy;
            r3.xy=(asuint(float4(r3.xyxx))).xy;
            r2.x=(asuint(mad(asfloat(r2.xxxx),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r0.yyyy))))).x;
            r0.y=(asuint(mad(asfloat(r1.xxxx),asfloat(r2.xxxx),asfloat(r0.yyyy)))).y;
            r2.x=(asuint(mad(asfloat(r3.xxxx),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r0.wwww))))).x;
            r0.w=(asuint(mad(asfloat(r1.xxxx),asfloat(r2.xxxx),asfloat(r0.wwww)))).w;
            r0.w=(asuint((-(asfloat(r0.yyyy)))+(asfloat(r0.wwww)))).w;
            r0.y=(asuint(mad(asfloat(r1.yyyy),asfloat(r0.wwww),asfloat(r0.yyyy)))).y;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(uint4(0x3e800000u,0x3e800000u,0x3e800000u,0x3e800000u))))).y;
            r0.x=(asuint(mad(asfloat(r0.xxxx),asfloat(uint4(0x3e000000u,0x3e000000u,0x3e000000u,0x3e000000u)),asfloat(r0.yyyy)))).x;
            r2.xyzw=(asuint((asfloat(r2.zwzw))+(asfloat(uint4(0x00000000u,0x3f800000u,0x3f800000u,0x3f800000u))))).xyzw;
            r2.xyzw=(asuint(int4(asfloat(r2.xyzw)))).xyzw;
            r0.yw=((r2.yyyw)^(uint4(0x00000000u,0x41c64e6du,0x00000000u,0x41c64e6du))).yw;
            r1.xy=((r0.ywyy)+(r2.xzxx)).xy;
            r0.yw=((r0.yyyw)*(r1.xxxy)).yw;
            r1.xy=((r0.ywyy)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).xy;
            r0.yw=((r0.yyyw)^(r1.xxxy)).yw;
            r0.yw=((r0.yyyw)*(uint4(0x00000000u,0x27d4eb2du,0x00000000u,0x27d4eb2du))).yw;
            r0.yw=((r0.yyyw)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).yw;
            r0.yw=(asuint(float4(r0.yyyw))).yw;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u))))).y;
            r1.x=(asuint(mad(asfloat(r3.yyyy),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r0.zzzz))))).x;
            r0.z=(asuint(mad(asfloat(r1.zzzz),asfloat(r1.xxxx),asfloat(r0.zzzz)))).z;
            r0.w=(asuint(mad(asfloat(r0.wwww),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r0.yyyy))))).w;
            r0.y=(asuint(mad(asfloat(r1.zzzz),asfloat(r0.wwww),asfloat(r0.yyyy)))).y;
            r0.y=(asuint((-(asfloat(r0.zzzz)))+(asfloat(r0.yyyy)))).y;
            r0.y=(asuint(mad(asfloat(r1.wwww),asfloat(r0.yyyy),asfloat(r0.zzzz)))).y;
            r0.x=(asuint(mad(asfloat(r0.yyyy),asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u)),asfloat(r0.xxxx)))).x;
            r0.x=(asuint(asfloat(uint4((asfloat(r0.xxxx))>=(cb1[1].xxxx))*0xffffffffu))).x;
            r0.x=((r0.xxxx)&(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u))).x;
            r1.w=(asuint((asfloat(r0.xxxx))*(cb1[1].yyyy))).w;
            r0.y=(asuint(asfloat(uint4((asfloat(r1.wwww))==(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).y;
            if(((r0.yyyy).x!=0u)) discard;
            r2.xyzw=(asuint((_MainTex.SampleBias(sampler_MainTex,(v1.xyxx).xy,(cb0[4].xxxx).x)).xyzw)).xyzw;
            r1.xyz=(asuint((asfloat(r0.xxxx))*(asfloat(r2.xyzx)))).xyz;
            o0.xyzw=((asfloat(r1.xyzw))*(v2.xyzw)).xyzw;
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
