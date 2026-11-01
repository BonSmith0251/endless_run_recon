Shader "Shader Graphs/UpgradeTreeBGShader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		_BaseColor ("BaseColor", Color) = (1,0,0.1443119,1)
		_SecondaryColor ("SecondaryColor", Color) = (0,0.01166201,1,1)
		_Edge_1 ("Edge 1", Float) = 0.3
		_Edge_2 ("Edge 2", Float) = 0.5
		_Center ("Center", Vector) = (-0.5,-0.5,0,0)
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		_Noise_Speed ("Noise Speed", Float) = 0.15
		_Noise_Scale ("Noise Scale", Float) = 0.8
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
        float4 _BaseColor;
        float4 _SecondaryColor;
        float _Edge_1;
        float _Edge_2;
        float2 _Center;
        float _Noise_Speed;
        float _Noise_Scale;
        CBUFFER_END
        Texture2D<float4> _MainTex; SamplerState sampler_MainTex;
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0,r2=0,r3=0,r4=0,r5=0;
            float4 cb0[20];
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
            float4 cb1[5];
            cb1[0]=0;
            cb1[1]=0;
            cb1[2]=0;
            cb1[3]=0;
            cb1[4]=0;
            cb0[4].xy=_GlobalMipBias;
            cb0[19].xyzw=_TimeParameters;
            cb1[0].xyzw=_BaseColor;
            cb1[1].xyzw=_SecondaryColor;
            cb1[2].x=_Edge_1;
            cb1[2].y=_Edge_2;
            cb1[2].zw=_Center;
            cb1[4].x=_Noise_Speed;
            cb1[4].y=_Noise_Scale;
            r0.xyzw=(asuint((_MainTex.SampleBias(sampler_MainTex,(v1.xyxx).xy,(cb0[4].xxxx).x)).xyzw)).xyzw;
            r1.x=(asuint(mad(cb0[19].xxxx,cb1[4].xxxx,v1.xxxx))).x;
            r1.yz=(asuint((v1.xxyx)*(cb1[4].yyyy))).yz;
            r2.xy=(asuint(floor(asfloat(r1.yzyy)))).xy;
            r1.yz=(asuint(frac(asfloat(r1.yyzy)))).yz;
            r2.zw=(uint4(0x00000000u,0x00000000u,0x00000000u,0x41000000u)).zw;
            r1.w=(uint4(0xffffffffu,0xffffffffu,0xffffffffu,0xffffffffu)).w;
            [loop] while(true) {
            r3.x=(uint4(asint(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))<asint(r1.wwww))*0xffffffffu).x;
            if(((r3.xxxx).x!=0u)) break;
            r3.y=(asuint(float4(asint(r1.wwww)))).y;
            r3.zw=(r2.zzzw).zw;
            r4.x=(uint4(0xffffffffu,0xffffffffu,0xffffffffu,0xffffffffu)).x;
            [loop] while(true) {
            r4.y=(uint4(asint(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))<asint(r4.xxxx))*0xffffffffu).y;
            if(((r4.yyyy).x!=0u)) break;
            r3.x=(asuint(float4(asint(r4.xxxx)))).x;
            r4.yz=(asuint((asfloat(r2.xxyx))+(asfloat(r3.xxyx)))).yz;
            r4.yz=(asuint(int4(asfloat(r4.yyzy)))).yz;
            r4.z=((r4.zzzz)^(uint4(0x41c64e6du,0x41c64e6du,0x41c64e6du,0x41c64e6du))).z;
            r4.y=((r4.zzzz)+(r4.yyyy)).y;
            r4.y=((r4.zzzz)*(r4.yyyy)).y;
            r4.w=((r4.yyyy)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).w;
            r4.y=((r4.wwww)^(r4.yyyy)).y;
            r5.y=((r4.yyyy)*(uint4(0x27d4eb2du,0x27d4eb2du,0x27d4eb2du,0x27d4eb2du))).y;
            r4.y=((r5.yyyy)<<(uint4(0x00000003u,0x00000003u,0x00000003u,0x00000003u))).y;
            r5.x=((r4.yyyy)^(r4.zzzz)).x;
            r4.yz=((r5.xxyx)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).yz;
            r4.yz=(asuint(float4(r4.yyzy))).yz;
            r4.yz=(asuint((asfloat(r1.xxxx))*(asfloat(r4.yyzy)))).yz;
            r4.yz=(asuint((asfloat(r4.yyzy))*(asfloat(uint4(0x00000000u,0x33800001u,0x33800001u,0x00000000u))))).yz;
            float4 trig0=asfloat(r4.yyyy);
            r5.x=(asuint(sin(trig0))).x;
            float4 trig1=asfloat(r4.zzzz);
            r5.y=(asuint(cos(trig1))).y;
            r4.yz=(asuint(mad(asfloat(r5.xxyx),asfloat(uint4(0x00000000u,0x3f000000u,0x3f000000u,0x00000000u)),asfloat(r3.xxyx)))).yz;
            r4.yz=(asuint((-(asfloat(r1.yyzy)))+(asfloat(r4.yyzy)))).yz;
            r4.yz=(asuint((asfloat(r4.yyzy))+(asfloat(uint4(0x00000000u,0x3f000000u,0x3f000000u,0x00000000u))))).yz;
            r3.x=(asuint(float4(dot((asfloat(r4.yzyy)).xy,(asfloat(r4.yzyy)).xy),dot((asfloat(r4.yzyy)).xy,(asfloat(r4.yzyy)).xy),dot((asfloat(r4.yzyy)).xy,(asfloat(r4.yzyy)).xy),dot((asfloat(r4.yzyy)).xy,(asfloat(r4.yzyy)).xy)))).x;
            r3.x=(asuint(sqrt(asfloat(r3.xxxx)))).x;
            r4.y=(asuint(asfloat(uint4((asfloat(r3.xxxx))<(asfloat(r3.wwww)))*0xffffffffu))).y;
            r3.zw=((r4.yyyy!=0u)?(r3.xxxx):(r3.zzzw)).zw;
            r4.x=((r4.xxxx)+(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))).x;
            }
            r2.zw=(r3.zzzw).zw;
            r1.w=((r1.wwww)+(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))).w;
            }
            r1.x=(asuint(asfloat(uint4((asfloat(r0.wwww))==(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).x;
            if(((r1.xxxx).x!=0u)) discard;
            r1.xy=(asuint((v1.xyxx)+(cb1[2].zwzz))).xy;
            r1.xy=(asuint(mad(asfloat(r1.xyxx),asfloat(r2.zzzz),asfloat(r1.xyxx)))).xy;
            r1.x=(asuint(float4(dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy)))).x;
            r1.x=(asuint(sqrt(asfloat(r1.xxxx)))).x;
            r1.y=(asuint((-(cb1[2].xxxx))+(cb1[2].yyyy))).y;
            r1.x=(asuint((asfloat(r1.xxxx))+(-(cb1[2].xxxx)))).x;
            r1.y=(asuint((asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))/(asfloat(r1.yyyy)))).y;
            r1.x=(asuint(saturate((asfloat(r1.yyyy))*(asfloat(r1.xxxx))))).x;
            r1.y=(asuint(mad(asfloat(r1.xxxx),asfloat(uint4(0xc0000000u,0xc0000000u,0xc0000000u,0xc0000000u)),asfloat(uint4(0x40400000u,0x40400000u,0x40400000u,0x40400000u))))).y;
            r1.x=(asuint((asfloat(r1.xxxx))*(asfloat(r1.xxxx)))).x;
            r1.x=(asuint((asfloat(r1.xxxx))*(asfloat(r1.yyyy)))).x;
            r1.yzw=(asuint((-(cb1[0].xxyz))+(cb1[1].xxyz))).yzw;
            r1.xyz=(asuint(mad(asfloat(r1.xxxx),asfloat(r1.yzwy),cb1[0].xyzx))).xyz;
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
