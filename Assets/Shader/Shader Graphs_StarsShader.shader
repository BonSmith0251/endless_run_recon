Shader "Shader Graphs/StarsShader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		_Speed ("Speed", Float) = 0.3
		_Density ("Density", Float) = 5
		_SmoothStep ("SmoothStep", Vector) = (0,0.15,0,0)
		_Color ("Color", Color) = (1,1,1,1)
		_Blend ("Blend", Range(0, 1)) = 1
		_Overlay_Noise_Scale ("Overlay Noise Scale", Float) = 4
		_Overlay_Noise_Speed ("Overlay Noise Speed", Float) = 0.05
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
        float _Speed;
        float _Density;
        float2 _SmoothStep;
        float4 _Color;
        float _Blend;
        float _Overlay_Noise_Scale;
        float _Overlay_Noise_Speed;
        CBUFFER_END
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0,r2=0,r3=0,r4=0;
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
            float4 cb1[3];
            cb1[0]=0;
            cb1[1]=0;
            cb1[2]=0;
            cb0[19].xyzw=_TimeParameters;
            cb1[0].x=_Speed;
            cb1[0].y=_Density;
            cb1[0].zw=_SmoothStep;
            cb1[1].xyzw=_Color;
            cb1[2].x=_Blend;
            cb1[2].y=_Overlay_Noise_Scale;
            cb1[2].z=_Overlay_Noise_Speed;
            r0.x=(asuint((cb0[19].xxxx)*(cb1[0].xxxx))).x;
            r0.yz=(asuint((v3.xxyx)*(cb1[0].yyyy))).yz;
            r1.xy=(asuint(floor(asfloat(r0.yzyy)))).xy;
            r0.yz=(asuint(frac(asfloat(r0.yyzy)))).yz;
            r1.zw=(uint4(0x00000000u,0x00000000u,0x00000000u,0x41000000u)).zw;
            r0.w=(uint4(0xffffffffu,0xffffffffu,0xffffffffu,0xffffffffu)).w;
            [loop] while(true) {
            r2.x=(uint4(asint(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))<asint(r0.wwww))*0xffffffffu).x;
            if(((r2.xxxx).x!=0u)) break;
            r2.y=(asuint(float4(asint(r0.wwww)))).y;
            r2.zw=(r1.zzzw).zw;
            r3.x=(uint4(0xffffffffu,0xffffffffu,0xffffffffu,0xffffffffu)).x;
            [loop] while(true) {
            r3.y=(uint4(asint(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))<asint(r3.xxxx))*0xffffffffu).y;
            if(((r3.yyyy).x!=0u)) break;
            r2.x=(asuint(float4(asint(r3.xxxx)))).x;
            r3.yz=(asuint((asfloat(r1.xxyx))+(asfloat(r2.xxyx)))).yz;
            r3.yz=(asuint(int4(asfloat(r3.yyzy)))).yz;
            r3.z=((r3.zzzz)^(uint4(0x41c64e6du,0x41c64e6du,0x41c64e6du,0x41c64e6du))).z;
            r3.y=((r3.zzzz)+(r3.yyyy)).y;
            r3.y=((r3.zzzz)*(r3.yyyy)).y;
            r3.w=((r3.yyyy)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).w;
            r3.y=((r3.wwww)^(r3.yyyy)).y;
            r4.y=((r3.yyyy)*(uint4(0x27d4eb2du,0x27d4eb2du,0x27d4eb2du,0x27d4eb2du))).y;
            r3.y=((r4.yyyy)<<(uint4(0x00000003u,0x00000003u,0x00000003u,0x00000003u))).y;
            r4.x=((r3.yyyy)^(r3.zzzz)).x;
            r3.yz=((r4.xxyx)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).yz;
            r3.yz=(asuint(float4(r3.yyzy))).yz;
            r3.yz=(asuint((asfloat(r0.xxxx))*(asfloat(r3.yyzy)))).yz;
            r3.yz=(asuint((asfloat(r3.yyzy))*(asfloat(uint4(0x00000000u,0x33800001u,0x33800001u,0x00000000u))))).yz;
            float4 trig0=asfloat(r3.yyyy);
            r4.x=(asuint(sin(trig0))).x;
            float4 trig1=asfloat(r3.zzzz);
            r4.y=(asuint(cos(trig1))).y;
            r3.yz=(asuint(mad(asfloat(r4.xxyx),asfloat(uint4(0x00000000u,0x3f000000u,0x3f000000u,0x00000000u)),asfloat(r2.xxyx)))).yz;
            r3.yz=(asuint((-(asfloat(r0.yyzy)))+(asfloat(r3.yyzy)))).yz;
            r3.yz=(asuint((asfloat(r3.yyzy))+(asfloat(uint4(0x00000000u,0x3f000000u,0x3f000000u,0x00000000u))))).yz;
            r2.x=(asuint(float4(dot((asfloat(r3.yzyy)).xy,(asfloat(r3.yzyy)).xy),dot((asfloat(r3.yzyy)).xy,(asfloat(r3.yzyy)).xy),dot((asfloat(r3.yzyy)).xy,(asfloat(r3.yzyy)).xy),dot((asfloat(r3.yzyy)).xy,(asfloat(r3.yzyy)).xy)))).x;
            r2.x=(asuint(sqrt(asfloat(r2.xxxx)))).x;
            r3.y=(asuint(asfloat(uint4((asfloat(r2.xxxx))<(asfloat(r2.wwww)))*0xffffffffu))).y;
            r2.zw=((r3.yyyy!=0u)?(r2.xxxx):(r2.zzzw)).zw;
            r3.x=((r3.xxxx)+(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))).x;
            }
            r1.zw=(r2.zzzw).zw;
            r0.w=((r0.wwww)+(uint4(0x00000001u,0x00000001u,0x00000001u,0x00000001u))).w;
            }
            r0.x=(asuint((-(cb1[0].zzzz))+(cb1[0].wwww))).x;
            r0.y=(asuint((asfloat(r1.zzzz))+(-(cb1[0].zzzz)))).y;
            r0.x=(asuint((asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))/(asfloat(r0.xxxx)))).x;
            r0.x=(asuint(saturate((asfloat(r0.xxxx))*(asfloat(r0.yyyy))))).x;
            r0.y=(asuint(mad(asfloat(r0.xxxx),asfloat(uint4(0xc0000000u,0xc0000000u,0xc0000000u,0xc0000000u)),asfloat(uint4(0x40400000u,0x40400000u,0x40400000u,0x40400000u))))).y;
            r0.x=(asuint((asfloat(r0.xxxx))*(asfloat(r0.xxxx)))).x;
            r0.x=(asuint(mad(-(asfloat(r0.yyyy)),asfloat(r0.xxxx),asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u))))).x;
            r0.yz=(asuint(mad(cb0[19].xxxx,cb1[2].zzzz,v3.xxyx))).yz;
            r0.yz=(asuint((asfloat(r0.yyzy))*(cb1[2].yyyy))).yz;
            r1.xy=(asuint(floor(asfloat(r0.yzyy)))).xy;
            r0.yz=(asuint(frac(asfloat(r0.yyzy)))).yz;
            r1.zw=(asuint(int4(asfloat(r1.xxxy)))).zw;
            r0.w=((r1.wwww)^(uint4(0x41c64e6du,0x41c64e6du,0x41c64e6du,0x41c64e6du))).w;
            r1.z=((r0.wwww)+(r1.zzzz)).z;
            r0.w=((r0.wwww)*(r1.zzzz)).w;
            r1.z=((r0.wwww)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).z;
            r0.w=((r0.wwww)^(r1.zzzz)).w;
            r0.w=((r0.wwww)*(uint4(0x27d4eb2du,0x27d4eb2du,0x27d4eb2du,0x27d4eb2du))).w;
            r0.w=((r0.wwww)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).w;
            r0.w=(asuint(float4(r0.wwww))).w;
            r2.yz=(asuint(mad(asfloat(r0.wwww),asfloat(uint4(0x00000000u,0x33800001u,0x33800001u,0x00000000u)),asfloat(uint4(0x00000000u,0x3f000000u,0xbf000000u,0x00000000u))))).yz;
            r1.z=(asuint(floor(asfloat(r2.yyyy)))).z;
            r2.x=(asuint(mad(asfloat(r0.wwww),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r1.zzzz))))).x;
            r0.w=(asuint(float4(dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy)))).w;
            r0.w=(asuint(rsqrt(asfloat(r0.wwww)))).w;
            r1.zw=(asuint((asfloat(r0.wwww))*(asfloat(r2.xxxz)))).zw;
            r0.w=(asuint(float4(dot((asfloat(r1.zwzz)).xy,(asfloat(r0.yzyy)).xy),dot((asfloat(r1.zwzz)).xy,(asfloat(r0.yzyy)).xy),dot((asfloat(r1.zwzz)).xy,(asfloat(r0.yzyy)).xy),dot((asfloat(r1.zwzz)).xy,(asfloat(r0.yzyy)).xy)))).w;
            r2.xyzw=(asuint((asfloat(r1.xyxy))+(asfloat(uint4(0x00000000u,0x3f800000u,0x3f800000u,0x00000000u))))).xyzw;
            r2.xyzw=(asuint(int4(asfloat(r2.xyzw)))).xyzw;
            r1.zw=((r2.yyyw)^(uint4(0x00000000u,0x00000000u,0x41c64e6du,0x41c64e6du))).zw;
            r2.xy=((r1.zwzz)+(r2.xzxx)).xy;
            r1.zw=((r1.zzzw)*(r2.xxxy)).zw;
            r2.xy=((r1.zwzz)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).xy;
            r1.zw=((r1.zzzw)^(r2.xxxy)).zw;
            r1.zw=((r1.zzzw)*(uint4(0x00000000u,0x00000000u,0x27d4eb2du,0x27d4eb2du))).zw;
            r1.zw=((r1.zzzw)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).zw;
            r1.zw=(asuint(float4(r1.zzzw))).zw;
            r2.xyzw=(asuint(mad(asfloat(r1.zwzw),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),asfloat(uint4(0x3f000000u,0x3f000000u,0xbf000000u,0xbf000000u))))).xyzw;
            r3.xy=(asuint(floor(asfloat(r2.xyxx)))).xy;
            r2.xy=(asuint(mad(asfloat(r1.zwzz),asfloat(uint4(0x33800001u,0x33800001u,0x00000000u,0x00000000u)),-(asfloat(r3.xyxx))))).xy;
            r1.z=(asuint(float4(dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy)))).z;
            r1.z=(asuint(rsqrt(asfloat(r1.zzzz)))).z;
            r1.zw=(asuint((asfloat(r1.zzzz))*(asfloat(r2.xxxz)))).zw;
            r3.xyzw=(asuint((asfloat(r0.yzyz))+(asfloat(uint4(0x80000000u,0xbf800000u,0xbf800000u,0x80000000u))))).xyzw;
            r1.z=(asuint(float4(dot((asfloat(r1.zwzz)).xy,(asfloat(r3.xyxx)).xy),dot((asfloat(r1.zwzz)).xy,(asfloat(r3.xyxx)).xy),dot((asfloat(r1.zwzz)).xy,(asfloat(r3.xyxx)).xy),dot((asfloat(r1.zwzz)).xy,(asfloat(r3.xyxx)).xy)))).z;
            r1.w=(asuint(float4(dot((asfloat(r2.ywyy)).xy,(asfloat(r2.ywyy)).xy),dot((asfloat(r2.ywyy)).xy,(asfloat(r2.ywyy)).xy),dot((asfloat(r2.ywyy)).xy,(asfloat(r2.ywyy)).xy),dot((asfloat(r2.ywyy)).xy,(asfloat(r2.ywyy)).xy)))).w;
            r1.w=(asuint(rsqrt(asfloat(r1.wwww)))).w;
            r2.xy=(asuint((asfloat(r1.wwww))*(asfloat(r2.ywyy)))).xy;
            r1.w=(asuint(float4(dot((asfloat(r2.xyxx)).xy,(asfloat(r3.zwzz)).xy),dot((asfloat(r2.xyxx)).xy,(asfloat(r3.zwzz)).xy),dot((asfloat(r2.xyxx)).xy,(asfloat(r3.zwzz)).xy),dot((asfloat(r2.xyxx)).xy,(asfloat(r3.zwzz)).xy)))).w;
            r1.xy=(asuint((asfloat(r1.xyxx))+(asfloat(uint4(0x3f800000u,0x3f800000u,0x00000000u,0x00000000u))))).xy;
            r1.xy=(asuint(int4(asfloat(r1.xyxx)))).xy;
            r1.y=((r1.yyyy)^(uint4(0x41c64e6du,0x41c64e6du,0x41c64e6du,0x41c64e6du))).y;
            r1.x=((r1.yyyy)+(r1.xxxx)).x;
            r1.x=((r1.yyyy)*(r1.xxxx)).x;
            r1.y=((r1.xxxx)>>(uint4(0x00000005u,0x00000005u,0x00000005u,0x00000005u))).y;
            r1.x=((r1.yyyy)^(r1.xxxx)).x;
            r1.x=((r1.xxxx)*(uint4(0x27d4eb2du,0x27d4eb2du,0x27d4eb2du,0x27d4eb2du))).x;
            r1.x=((r1.xxxx)>>(uint4(0x00000008u,0x00000008u,0x00000008u,0x00000008u))).x;
            r1.x=(asuint(float4(r1.xxxx))).x;
            r2.yz=(asuint(mad(asfloat(r1.xxxx),asfloat(uint4(0x00000000u,0x33800001u,0x33800001u,0x00000000u)),asfloat(uint4(0x00000000u,0x3f000000u,0xbf000000u,0x00000000u))))).yz;
            r1.y=(asuint(floor(asfloat(r2.yyyy)))).y;
            r2.x=(asuint(mad(asfloat(r1.xxxx),asfloat(uint4(0x33800001u,0x33800001u,0x33800001u,0x33800001u)),-(asfloat(r1.yyyy))))).x;
            r1.x=(asuint(float4(dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy),dot((asfloat(r2.xzxx)).xy,(asfloat(r2.xzxx)).xy)))).x;
            r1.x=(asuint(rsqrt(asfloat(r1.xxxx)))).x;
            r1.xy=(asuint((asfloat(r1.xxxx))*(asfloat(r2.xzxx)))).xy;
            r2.xy=(asuint((asfloat(r0.yzyy))+(asfloat(uint4(0xbf800000u,0xbf800000u,0x00000000u,0x00000000u))))).xy;
            r1.x=(asuint(float4(dot((asfloat(r1.xyxx)).xy,(asfloat(r2.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r2.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r2.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r2.xyxx)).xy)))).x;
            r2.xy=(asuint((asfloat(r0.yzyy))*(asfloat(r0.yzyy)))).xy;
            r2.xy=(asuint((asfloat(r0.yzyy))*(asfloat(r2.xyxx)))).xy;
            r2.zw=(asuint(mad(asfloat(r0.yyyz),asfloat(uint4(0x00000000u,0x00000000u,0x40c00000u,0x40c00000u)),asfloat(uint4(0x00000000u,0x00000000u,0xc1700000u,0xc1700000u))))).zw;
            r0.yz=(asuint(mad(asfloat(r0.yyzy),asfloat(r2.zzwz),asfloat(uint4(0x00000000u,0x41200000u,0x41200000u,0x00000000u))))).yz;
            r0.yz=(asuint((asfloat(r0.yyzy))*(asfloat(r2.xxyx)))).yz;
            r1.y=(asuint((-(asfloat(r0.wwww)))+(asfloat(r1.zzzz)))).y;
            r0.w=(asuint(mad(asfloat(r0.zzzz),asfloat(r1.yyyy),asfloat(r0.wwww)))).w;
            r1.x=(asuint((-(asfloat(r1.wwww)))+(asfloat(r1.xxxx)))).x;
            r0.z=(asuint(mad(asfloat(r0.zzzz),asfloat(r1.xxxx),asfloat(r1.wwww)))).z;
            r0.z=(asuint((-(asfloat(r0.wwww)))+(asfloat(r0.zzzz)))).z;
            r0.y=(asuint(mad(asfloat(r0.yyyy),asfloat(r0.zzzz),asfloat(r0.wwww)))).y;
            r0.y=(asuint((asfloat(r0.yyyy))+(asfloat(uint4(0x3e4cccccu,0x3e4cccccu,0x3e4cccccu,0x3e4cccccu))))).y;
            r0.y=(asuint(saturate((asfloat(r0.yyyy))*(asfloat(uint4(0x3fb6db6eu,0x3fb6db6eu,0x3fb6db6eu,0x3fb6db6eu)))))).y;
            r0.z=(asuint(mad(asfloat(r0.yyyy),asfloat(uint4(0xc0000000u,0xc0000000u,0xc0000000u,0xc0000000u)),asfloat(uint4(0x40400000u,0x40400000u,0x40400000u,0x40400000u))))).z;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(r0.yyyy)))).y;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(r0.zzzz)))).y;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(r0.xxxx)))).y;
            r1.w=(asuint((asfloat(r0.yyyy))*(cb1[2].xxxx))).w;
            r0.y=(asuint(asfloat(uint4((asfloat(r1.wwww))==(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).y;
            if(((r0.yyyy).x!=0u)) discard;
            r1.xyz=(asuint((asfloat(r0.xxxx))*(cb1[1].xyzx))).xyz;
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
