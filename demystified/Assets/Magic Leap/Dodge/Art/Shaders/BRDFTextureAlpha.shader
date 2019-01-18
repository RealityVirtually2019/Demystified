// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

 Shader "Magic Leap/BRDF/TextureAlpha"
 {
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Color Map/Alpha Spec", 2D) = "white" {}
		_RampLight ("RampLight", 2D) = "Black" {}
		_Multi ("Fade", Range(0.0,2.0)) = 1.0
		_LightVector ("LightVector", Vector) = (0,0,0,0)
		_Alpha ("Fade", Range(0.0,1.0)) = 1.0
    }

	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType" = "Transparent" }
		LOD 200
		ZWrite on
		Lighting Off
		Cull back
		ZTest LEqual
		Fog { Mode Off}
		CGPROGRAM
		#pragma surface surf RampLight alpha novertexlights exclude_path:prepass noambient noforwardadd nolightmap nodirlightmap
		#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_Ramp;
		};

		sampler2D _MainTex;
		sampler2D _RampLight;
		half _Luma;
		half _Multi;
		half _Alpha;
		float4 _Color;
		float4 _LightVector;

		half4 LightingRampLight (SurfaceOutput s, half3 lightDir, half3 viewDir, fixed atten)
		{
			float light = dot(s.Normal,_LightVector);
			float rim = dot(s.Normal,viewDir);
			float diff = (light*.5)+.5;
				
			float2 brdfdiff = float2(rim, diff);
			float3 BRDFLight = tex2D(_RampLight, brdfdiff.xy).rgb;
		
			half3 BRDFColor = (s.Albedo*_Color);
			
      
			half4 c;
			c.rgb =BRDFColor*BRDFLight*_Multi;
			c.a = _Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			half3 maintex = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = maintex;
		}
		ENDCG
	}

    Fallback "Diffuse"
}