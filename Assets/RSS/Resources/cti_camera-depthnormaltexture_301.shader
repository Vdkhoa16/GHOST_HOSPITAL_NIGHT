Shader "Hidden/CTI_Camera-DepthNormalTexture_301" {
	Properties {
		_MainTex ("", 2D) = "white" {}
		_Cutoff ("", Float) = 0.5
		_Color ("", Vector) = (1,1,1,1)
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}