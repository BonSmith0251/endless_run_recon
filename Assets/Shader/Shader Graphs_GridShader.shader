Shader "Shader Graphs/GridShader" {
	Properties {
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[ToggleUI] _Vertical_Lines ("Vertical Lines", Float) = 1
		_Line_Width ("Line Width", Float) = 0.05
		_Size ("Size", Float) = 10
		_Color ("Color", Color) = (0,0,0,1)
		_Color_1 ("Color (1)", Color) = (0,0.0295701,1,1)
		_Offset ("Offset", Float) = 4.22
		_GradientStep1 ("GradientStep1", Float) = 0
		_GradientStep2 ("GradientStep2", Float) = 1
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
        float _Vertical_Lines;
        float _Line_Width;
        float _Size;
        float4 _Color;
        float4 _Color_1;
        float _Offset;
        float _GradientStep1;
        float _GradientStep2;
        CBUFFER_END
        float4 frag(Varyings i):SV_Target {
            float4 v0=i.positionCS,v1=float4(i.uv,0,0),v2=i.color,v3=float4(i.positionWS,0),v4=0,o0=0;
            uint4 r0=0,r1=0,r2=0;
            float4 cb0[4];
            cb0[0]=0;
            cb0[1]=0;
            cb0[2]=0;
            cb0[3]=0;
            cb0[0].x=_Vertical_Lines;
            cb0[0].y=_Line_Width;
            cb0[0].z=_Size;
            cb0[1].xyzw=_Color;
            cb0[2].xyzw=_Color_1;
            cb0[3].x=_Offset;
            cb0[3].y=_GradientStep1;
            cb0[3].z=_GradientStep2;
            r0.xy=(asuint((-(cb0[0].yyyy))+(asfloat(uint4(0x3f000000u,0x3f800000u,0x00000000u,0x00000000u))))).xy;
            r0.zw=(asuint(mad(cb0[3].xxxx,asfloat(uint4(0x00000000u,0x00000000u,0xbf800000u,0x3f000000u)),v3.xxxy))).zw;
            r0.zw=(asuint((asfloat(r0.zzzw))*(cb0[0].zzzz))).zw;
            r1.x=(asuint(frac(asfloat(r0.zzzz)))).x;
            r1.x=(asuint((asfloat(r1.xxxx))+(asfloat(uint4(0xbf000000u,0xbf000000u,0xbf000000u,0xbf000000u))))).x;
            r1.y=(asuint((-(asfloat(r0.xxxx)))+(asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u))))).y;
            r0.x=(asuint((-(asfloat(r0.xxxx)))+(abs(asfloat(r1.xxxx))))).x;
            r1.x=(asuint((asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))/(asfloat(r1.yyyy)))).x;
            r0.x=(asuint(saturate((asfloat(r0.xxxx))*(asfloat(r1.xxxx))))).x;
            r1.x=(asuint(mad(asfloat(r0.xxxx),asfloat(uint4(0xc0000000u,0xc0000000u,0xc0000000u,0xc0000000u)),asfloat(uint4(0x40400000u,0x40400000u,0x40400000u,0x40400000u))))).x;
            r0.x=(asuint((asfloat(r0.xxxx))*(asfloat(r0.xxxx)))).x;
            r0.x=(asuint((asfloat(r0.xxxx))*(asfloat(r1.xxxx)))).x;
            r1.x=(asuint(asfloat(uint4((asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u)))!=(cb0[0].xxxx))*0xffffffffu))).x;
            r0.x=((r0.xxxx)&(r1.xxxx)).x;
            r0.zw=(asuint(mad(asfloat(r0.zzzz),asfloat(uint4(0x00000000u,0x00000000u,0x3f000000u,0xbf000000u)),asfloat(r0.wwww)))).zw;
            r0.zw=(asuint(frac(asfloat(r0.zzzw)))).zw;
            r0.yz=(asuint(asfloat(uint4((asfloat(r0.zzwz))>=(asfloat(r0.yyyy)))*0xffffffffu))).yz;
            r0.yz=((r0.yyzy)&(uint4(0x00000000u,0x3f800000u,0x3f800000u,0x00000000u))).yz;
            r0.y=(asuint(max(asfloat(r0.zzzz),asfloat(r0.yyyy)))).y;
            r0.x=(asuint(max(asfloat(r0.yyyy),asfloat(r0.xxxx)))).x;
            r0.y=(asuint((-(v3.xxxx))+(v3.yyyy))).y;
            r0.y=(asuint(mad(asfloat(r0.yyyy),asfloat(uint4(0x3f000000u,0x3f000000u,0x3f000000u,0x3f000000u)),v3.xxxx))).y;
            r0.z=(asuint((-(cb0[3].yyyy))+(cb0[3].zzzz))).z;
            r0.y=(asuint((asfloat(r0.yyyy))+(-(cb0[3].yyyy)))).y;
            r0.z=(asuint((asfloat(uint4(0x3f800000u,0x3f800000u,0x3f800000u,0x3f800000u)))/(asfloat(r0.zzzz)))).z;
            r0.y=(asuint(saturate((asfloat(r0.zzzz))*(asfloat(r0.yyyy))))).y;
            r0.z=(asuint(mad(asfloat(r0.yyyy),asfloat(uint4(0xc0000000u,0xc0000000u,0xc0000000u,0xc0000000u)),asfloat(uint4(0x40400000u,0x40400000u,0x40400000u,0x40400000u))))).z;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(r0.yyyy)))).y;
            r0.y=(asuint((asfloat(r0.yyyy))*(asfloat(r0.zzzz)))).y;
            r0.z=(asuint((-(cb0[1].wwww))+(cb0[2].wwww))).z;
            r0.z=(asuint(mad(asfloat(r0.yyyy),asfloat(r0.zzzz),cb0[1].wwww))).z;
            r1.w=(asuint((asfloat(r0.zzzz))*(asfloat(r0.xxxx)))).w;
            r0.z=(asuint(asfloat(uint4((asfloat(r1.wwww))==(asfloat(uint4(0x00000000u,0x00000000u,0x00000000u,0x00000000u))))*0xffffffffu))).z;
            if(((r0.zzzz).x!=0u)) discard;
            r2.xyz=(asuint((-(cb0[1].xyzx))+(cb0[2].xyzx))).xyz;
            r0.yzw=(asuint(mad(asfloat(r0.yyyy),asfloat(r2.xxyz),cb0[1].xxyz))).yzw;
            r1.xyz=(asuint((asfloat(r0.yzwy))*(asfloat(r0.xxxx)))).xyz;
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
