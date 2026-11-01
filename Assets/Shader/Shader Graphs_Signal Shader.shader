Shader "Shader Graphs/Signal Shader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
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
        
        CBUFFER_END
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0,r2=0,r3=0;
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
            cb0[19].xyzw=_TimeParameters;
            r0.xy=(asuint(mad(v1.xyxx,asfloat(uint4(0x40000000u,0x40000000u,0x00000000u,0x00000000u)),asfloat(uint4(0xbf800000u,0xbf800000u,0x00000000u,0x00000000u))))).xy;
            r0.x=(asuint(float4(dot((asfloat(r0.xyxx)).xy,(asfloat(r0.xyxx)).xy),dot((asfloat(r0.xyxx)).xy,(asfloat(r0.xyxx)).xy),dot((asfloat(r0.xyxx)).xy,(asfloat(r0.xyxx)).xy),dot((asfloat(r0.xyxx)).xy,(asfloat(r0.xyxx)).xy)))).x;
            r0.x=(asuint(sqrt(asfloat(r0.xxxx)))).x;
            r0.y=(asuint((asfloat(r0.xxxx))+(asfloat(uint4(0xbf800000u,0xbf800000u,0xbf800000u,0xbf800000u))))).y;
            r1.w=(asuint(saturate(mad(-(asfloat(r0.yyyy)),asfloat(uint4(0x42c80000u,0x42c80000u,0x42c80000u,0x42c80000u)),asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))))).w;
            r0.z=(asuint(asfloat(uint4((asfloat(r1.wwww))==(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).z;
            if(((r0.zzzz).x!=0u)) discard;
            r0.zw=(asuint((cb0[19].xxxx)*(asfloat(uint4(0x00000000u,0x00000000u,0xbecccccdu,0x3f000000u))))).zw;
            r0.z=(asuint(frac(asfloat(r0.zzzz)))).z;
            r0.z=(asuint((asfloat(r0.xxxx))+(asfloat(r0.zzzz)))).z;
            r0.z=(asuint((asfloat(r0.zzzz))*(asfloat(uint4(0x41460bc6u,0x41460bc6u,0x41460bc6u,0x41460bc6u))))).z;
            float4 trig0=asfloat(r0.zzzz);
            r0.z=(asuint(sin(trig0))).z;
            r0.z=(asuint(mad(asfloat(r0.zzzz),asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u)),asfloat(uint4(0x3e570a3eu,0x3e570a3eu,0x3e570a3eu,0x3e570a3eu))))).z;
            r0.z=(asuint(saturate((asfloat(r0.zzzz))*(asfloat(uint4(0x4247fff9u,0x4247fff9u,0x4247fff9u,0x4247fff9u)))))).z;
            r0.x=(asuint((-(asfloat(r0.xxxx)))+(asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u))))).x;
            r0.x=(asuint(-(asfloat(r0.xxxx)))).x;
            r2.x=(asuint(ddx(asfloat(r0.xxxx)))).x;
            r2.y=(asuint(ddy(asfloat(r0.xxxx)))).y;
            r2.x=(asuint(float4(dot((asfloat(r2.xyxx)).xy,(asfloat(r2.xyxx)).xy),dot((asfloat(r2.xyxx)).xy,(asfloat(r2.xyxx)).xy),dot((asfloat(r2.xyxx)).xy,(asfloat(r2.xyxx)).xy),dot((asfloat(r2.xyxx)).xy,(asfloat(r2.xyxx)).xy)))).x;
            r2.x=(asuint(sqrt(asfloat(r2.xxxx)))).x;
            r0.x=(asuint((asfloat(r0.xxxx))/(asfloat(r2.xxxx)))).x;
            r0.x=(asuint(saturate((-(asfloat(r0.xxxx)))+(asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u)))))).x;
            r2.x=(asuint((asfloat(r0.xxxx))*(asfloat(r0.zzzz)))).x;
            r0.x=(asuint(mad(-(asfloat(r0.zzzz)),asfloat(r0.xxxx),asfloat(r1.wwww)))).x;
            r2.yzw=(asuint((asfloat(r0.xxxx))*(asfloat(uint4(0x00000000u,0x3f752631u,0x3e71933cu,0x3e71933cu))))).yzw;
            r2.xyz=(asuint(mad(asfloat(r2.xxxx),asfloat(uint4(0x3f800000u,0x3e020b0eu,0x3e020b0eu,0x00000000u)),asfloat(r2.yzwy)))).xyz;
            r0.x=(asuint((abs(asfloat(r0.yyyy)))+(asfloat(uint4(0xbe0f5c29u,0xbe0f5c29u,0xbe0f5c29u,0xbe0f5c29u))))).x;
            r0.x=(asuint(max(asfloat(r0.xxxx),asfloat(r0.yyyy)))).x;
            r0.x=(asuint(saturate(mad(-(asfloat(r0.xxxx)),asfloat(uint4(0x42c80000u,0x42c80000u,0x42c80000u,0x42c80000u)),asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))))).x;
            r0.yz=(asuint((v1.xxyx)+(asfloat(uint4(0x00000000u,0xbf000000u,0xbf000000u,0x00000000u))))).yz;
            r2.w=(asuint(min(abs(asfloat(r0.zzzz)),abs(asfloat(r0.yyyy))))).w;
            r3.x=(asuint(max(abs(asfloat(r0.zzzz)),abs(asfloat(r0.yyyy))))).x;
            r3.x=(asuint((asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))/(asfloat(r3.xxxx)))).x;
            r2.w=(asuint((asfloat(r2.wwww))*(asfloat(r3.xxxx)))).w;
            r3.x=(asuint((asfloat(r2.wwww))*(asfloat(r2.wwww)))).x;
            r3.y=(asuint(mad(asfloat(r3.xxxx),asfloat(uint4(0x3caaae5fu,0x3caaae5fu,0x3caaae5fu,0x3caaae5fu)),asfloat(uint4(0xbdae5a36u,0xbdae5a36u,0xbdae5a36u,0xbdae5a36u))))).y;
            r3.y=(asuint(mad(asfloat(r3.xxxx),asfloat(r3.yyyy),asfloat(uint4(0x3e3876e2u,0x3e3876e2u,0x3e3876e2u,0x3e3876e2u))))).y;
            r3.y=(asuint(mad(asfloat(r3.xxxx),asfloat(r3.yyyy),asfloat(uint4(0xbea91d04u,0xbea91d04u,0xbea91d04u,0xbea91d04u))))).y;
            r3.x=(asuint(mad(asfloat(r3.xxxx),asfloat(r3.yyyy),asfloat(uint4(0x3f7ff738u,0x3f7ff738u,0x3f7ff738u,0x3f7ff738u))))).x;
            r3.y=(asuint((asfloat(r2.wwww))*(asfloat(r3.xxxx)))).y;
            r3.z=(asuint(asfloat(uint4((abs(asfloat(r0.zzzz)))<(abs(asfloat(r0.yyyy))))*0xffffffffu))).z;
            r3.y=(asuint(mad(asfloat(r3.yyyy),asfloat(uint4(0xc0000000u,0xc0000000u,0xc0000000u,0xc0000000u)),asfloat(uint4(0x3fc90fdbu,0x3fc90fdbu,0x3fc90fdbu,0x3fc90fdbu))))).y;
            r3.y=((r3.zzzz)&(r3.yyyy)).y;
            r2.w=(asuint(mad(asfloat(r2.wwww),asfloat(r3.xxxx),asfloat(r3.yyyy)))).w;
            r3.x=(asuint(asfloat(uint4((asfloat(r0.zzzz))<(-(asfloat(r0.zzzz))))*0xffffffffu))).x;
            r3.x=((r3.xxxx)&(uint4(0xc0490fdbu,0xc0490fdbu,0xc0490fdbu,0xc0490fdbu))).x;
            r2.w=(asuint((asfloat(r2.wwww))+(asfloat(r3.xxxx)))).w;
            r3.x=(asuint(min(asfloat(r0.zzzz),asfloat(r0.yyyy)))).x;
            r0.y=(asuint(max(asfloat(r0.zzzz),asfloat(r0.yyyy)))).y;
            r0.z=(asuint(asfloat(uint4((asfloat(r3.xxxx))<(-(asfloat(r3.xxxx))))*0xffffffffu))).z;
            r0.y=(asuint(asfloat(uint4((asfloat(r0.yyyy))>=(-(asfloat(r0.yyyy))))*0xffffffffu))).y;
            r0.y=((r0.yyyy)&(r0.zzzz)).y;
            r0.y=((r0.yyyy!=0u)?(asuint(-(asfloat(r2.wwww)))):(r2.wwww)).y;
            r0.y=(asuint(mad(asfloat(r0.yyyy),asfloat(uint4(0x3e230eacu,0x3e230eacu,0x3e230eacu,0x3e230eacu)),asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u))))).y;
            r0.y=(asuint(mad(asfloat(r0.yyyy),asfloat(uint4(0x40000000u,0x40000000u,0x40000000u,0x40000000u)),-(asfloat(r0.wwww))))).y;
            r0.y=(asuint(frac(asfloat(r0.yyyy)))).y;
            r0.y=(asuint(log2(asfloat(r0.yyyy)))).y;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(uint4(0x40128f5cu,0x40128f5cu,0x40128f5cu,0x40128f5cu))))).y;
            r0.y=(asuint(exp2(asfloat(r0.yyyy)))).y;
            r1.xyz=(asuint(mad(asfloat(r0.xxxx),asfloat(r0.yyyy),asfloat(r2.xyzx)))).xyz;
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
