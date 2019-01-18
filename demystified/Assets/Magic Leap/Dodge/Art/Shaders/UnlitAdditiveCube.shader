// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

Shader "Magic Leap/Unlit/AdditiveCube"
{
	Properties 
	{
		_Color ("Main Color",Color)= (0,0,0,0)
		_RefCube ("Cubemap", CUBE) = "" {}
		_Multi ("Multiplier", Range(0.0,2)) = 0.0
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType" = "Transparent" }
		LOD 200
		ZWrite Off
		Lighting Off
		Cull off
		ZTest LEqual
		Blend One One 
		Fog { Mode Off}

		CGPROGRAM
		#pragma surface surf Additive  halfasview novertexlights exclude_path:prepass noambient noforwardadd nolightmap nodirlightmap

		half4 LightingAdditive (SurfaceOutput s, half3 lightDir, half3 viewDir)
  	    {
			half3 h = normalize (lightDir + viewDir);

			half4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		struct Input
		{
			float3 worldRefl;
			INTERNAL_DATA
		};
      
		float _Multi;
		float3 _Color;
		samplerCUBE _RefCube;

		void surf (Input IN, inout SurfaceOutput o)
		{
			float3 refcube = texCUBE(_RefCube, WorldReflectionVector(IN, o.Normal)).rgb*_Color;
			o.Emission = (_Color+refcube)*(_Multi);  
		}
		ENDCG
	} 

	Fallback "Diffuse"
}