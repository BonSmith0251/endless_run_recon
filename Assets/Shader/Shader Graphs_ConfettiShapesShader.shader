Shader "Shader Graphs/ConfettiShapesShader" {
	Properties {
		[ToggleUI] _UseRectangle ("UseRectangle", Float) = 1
		[ToggleUI] _UseSpiral ("UseSpiral", Float) = 0
		[ToggleUI] _UseStar ("UseStar", Float) = 0
		[ToggleUI] _UseHollowStar ("UseHollowStar", Float) = 0
		[ToggleUI] _UseBlob ("UseBlob", Float) = 0
		[ToggleUI] _UseHeart ("UseHeart", Float) = 0
		_Spirals ("Spirals", Float) = 15
		_Tickness ("Tickness", Range(1, 2)) = 1.2
		[ToggleUI] _UV_Distortion ("UV_Distortion", Float) = 0
		_Speed ("Speed", Float) = 0.2
		_DistortAmount ("DistortAmount", Float) = 10
		_SoftParticlesAmount ("SoftParticlesAmount", Float) = 0.1
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}