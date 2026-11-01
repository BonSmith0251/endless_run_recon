Shader "Reconstruction/Original Sidebar Gradient" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Alpha ("Alpha", Float) = 1
        _GradBlend ("Gradient Blend", Float) = 1
        _GradBoostY ("Gradient Boost Y", Float) = 1.2
        _GradTopLeftCol ("Gradient Top", Color) = (1,1,1,1)
        _GradBotLeftCol ("Gradient Bottom", Color) = (0,0,0,1)
        _MySrcMode ("SrcMode", Float) = 5
        _MyDstMode ("DstMode", Float) = 10
        _ZTestMode ("Z Test Mode", Float) = 4
        _ZWrite ("Depth Write", Float) = 0
        _CullingOption ("Culling Option", Float) = 0
    }
    // Original AllIn1 variant, sharedassets1 Shader 373, vertex blob 7 / fragment blob 23.
    // The source material is the only consumer. Other AllIn1 materials retain their existing shader.
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "CanUseSpriteAtlas"="True" }
        Blend [_MySrcMode] [_MyDstMode]
        ZWrite [_ZWrite] ZTest [_ZTestMode] Cull [_CullingOption]
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature_local _ GRADIENT_ON
            #pragma shader_feature_local _ GRADIENT2COL_ON
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_ST, _Color, _GradTopLeftCol, _GradBotLeftCol;
            float _Alpha, _GradBlend, _GradBoostY;
            struct Attributes { float4 vertex:POSITION; float2 uv:TEXCOORD0; float4 color:COLOR; };
            struct Varyings { float2 uv:TEXCOORD0; float4 vertex:SV_POSITION; float4 color:COLOR; };
            Varyings vert(Attributes i) {
                Varyings o; o.vertex=UnityObjectToClipPos(i.vertex);
                o.uv=i.uv*_MainTex_ST.xy+_MainTex_ST.zw; o.color=i.color; return o;
            }
            float4 frag(Varyings i):SV_Target {
                float t=min(exp2(log2(i.uv.y/_MainTex_ST.y)*_GradBoostY),1.0);
                float4 g=mad(t,_GradTopLeftCol-_GradBotLeftCol,_GradBotLeftCol);
                float4 c=tex2D(_MainTex,i.uv)*i.color;
                float4 result=mad(_GradBlend,g-c,c)*c.a;
                result.a*=_Alpha;
                return result*_Color;
            }
            ENDCG
        }
    }
}
