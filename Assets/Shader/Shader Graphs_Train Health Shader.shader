Shader "Shader Graphs/Train Health Shader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		_Rotation ("Rotation", Float) = -45
		_Bars ("Bars", Float) = 4
		_Edge_Width ("Edge Width", Range(0, 1)) = 0.2
		_EdgeColor ("EdgeColor", Color) = (1,1,1,1)
		_BarColor ("BarColor", Color) = (1,1,1,1)
		_Size ("Size", Vector) = (100,100,0,0)
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
        float _Rotation;
        float _Bars;
        float _Edge_Width;
        float4 _BarColor;
        float2 _Size;
        CBUFFER_END
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0;
            float4 cb0[4];
            cb0[0]=0;
            cb0[1]=0;
            cb0[2]=0;
            cb0[3]=0;
            cb0[0].x=_Rotation;
            cb0[0].y=_Bars;
            cb0[0].z=_Edge_Width;
            cb0[2].xyzw=_BarColor;
            cb0[3].xy=_Size;
            r0.x=(asuint((cb0[0].xxxx)*(asfloat(uint4(0x3c8efa35u,0x3c8efa35u,0x3c8efa35u,0x3c8efa35u))))).x;
            r0.yz=(asuint((v1.xxyx)+(asfloat(uint4(0x00000000u,0xbf000000u,0xbf000000u,0x00000000u))))).yz;
            float4 trig0=asfloat(r0.xxxx);
            r0.x=(asuint(sin(trig0))).x;
            r1.x=(asuint(cos(trig0))).x;
            r1.y=(r0.xxxx).y;
            r0.x=(asuint(float4(dot((asfloat(r0.yzyy)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r0.yzyy)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r0.yzyy)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r0.yzyy)).xy,(asfloat(r1.xyxx)).xy)))).x;
            r0.x=(asuint((asfloat(r0.xxxx))+(asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u))))).x;
            r0.x=(asuint((asfloat(r0.xxxx))*(cb0[0].yyyy))).x;
            r0.x=(asuint(floor(asfloat(r0.xxxx)))).x;
            r0.x=(asuint((asfloat(r0.xxxx))*(asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u))))).x;
            r0.y=(asuint(asfloat(uint4((asfloat(r0.xxxx))>=(-(asfloat(r0.xxxx))))*0xffffffffu))).y;
            r0.x=(asuint(frac(abs(asfloat(r0.xxxx))))).x;
            r0.x=((r0.yyyy!=0u)?(r0.xxxx):(asuint(-(asfloat(r0.xxxx))))).x;
            r0.yz=(asuint(mad(v1.xxyx,asfloat(uint4(0x00000000u,0x40000000u,0x40000000u,0x00000000u)),asfloat(uint4(0x00000000u,0xbf800000u,0xbf800000u,0x00000000u))))).yz;
            r0.w=(asuint((-(cb0[0].zzzz))+(asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u))))).w;
            r1.x=(asuint((cb0[0].zzzz)+(asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u))))).x;
            r1.x=(asuint((-(asfloat(r0.wwww)))+(asfloat(r1.xxxx)))).x;
            r0.w=(asuint(mad(asfloat(r1.xxxx),asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u)),asfloat(r0.wwww)))).w;
            r0.yz=(asuint((-(asfloat(r0.wwww)))+(abs(asfloat(r0.yyzy))))).yz;
            r1.xy=(asuint((cb0[3].xyxx)/(cb0[3].yxyy))).xy;
            r1.xy=(asuint(max(asfloat(r1.xyxx),asfloat(uint4(0x3f800000u,0x3f800000u,0x00000000u,0x00000000u))))).xy;
            r0.yz=(asuint((asfloat(r0.yyzy))*(asfloat(r1.xxyx)))).yz;
            r1.xy=(asuint(max(asfloat(r0.yzyy),asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))).xy;
            r0.w=(asuint(float4(dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy),dot((asfloat(r1.xyxx)).xy,(asfloat(r1.xyxx)).xy)))).w;
            r0.w=(asuint(sqrt(asfloat(r0.wwww)))).w;
            r0.y=(asuint(max(asfloat(r0.zzzz),asfloat(r0.yyyy)))).y;
            r0.y=(asuint(min(asfloat(r0.yyyy),asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))).y;
            r0.xy=(asuint((asfloat(r0.xyxx))+(asfloat(r0.xwxx)))).xy;
            r0.z=(asuint((abs(asfloat(r0.yyyy)))+(-(cb0[0].zzzz)))).z;
            r0.y=(asuint(max(asfloat(r0.zzzz),asfloat(r0.yyyy)))).y;
            r0.y=(asuint(saturate(mad(-(asfloat(r0.yyyy)),asfloat(uint4(0x42c80000u,0x42c80000u,0x42c80000u,0x42c80000u)),asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))))).y;
            r0.x=(asuint((-(asfloat(r0.yyyy)))+(abs(asfloat(r0.xxxx))))).x;
            r1.xyzw=(asuint(saturate((asfloat(r0.xxxx))*(cb0[2].xyzx)))).xyzw;
            r0.xyzw=(asuint((asfloat(r0.yyyy))+(asfloat(r1.xyzw)))).xyzw;
            r1.x=(asuint(asfloat(uint4((asfloat(r0.wwww))==(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).x;
            if(((r1.xxxx).x!=0u)) discard;
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
