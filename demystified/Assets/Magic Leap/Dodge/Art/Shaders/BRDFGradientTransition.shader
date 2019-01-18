// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

 Shader "Magic Leap/BRDF/GradientWipeTransition" {
    Properties {
      
      _Color        ("Main Color",            Color)          = (1,1,1,1)
      _TransColor        ("Transition Color",            Color)          = (1,1,1,1)
      
      _MainTex ("Main Texture", 2D) = "white" {}
      _AltTex ("Alternate Texture", 2D) = "white" {}
      _TransTex ("Transition Map", 2D) = "white" {}
      
      _RampLight ("RampLight", 2D) = "Black" {}
      _RampDark ("RampDark", 2D) = "Black" {}
      
	  _Brightness ("Brightness", Range(-1.0,.5)) = 1.0
	  _Fade ("Fade", Range(0.0,2.0)) = 1.0
	  _Trans("Transition", Range(0.0,1.0)) = 0.0
	  _Luma ("LumaBoost", Range(-1.0,2.0)) = 1.0
	  _LightVector ("LightVector", Vector) = (0,0,0,0)
	  
	  

    }
    SubShader {
      Tags { "RenderType" = "opaque" "Queue"="geometry"}
      
      Lighting Off
      CGPROGRAM
       #pragma surface surf RampLight novertexlights exclude_path:prepass noambient noforwardadd nolightmap nodirlightmap
	  #pragma target 3.0
	  struct Input {
          float2 uv_MainTex;
          float2 uv_Ramp;
          float2 uv_TransTex;

  

      };
      sampler2D _MainTex;
      sampler2D _AltTex;
      sampler2D _TransTex;
      sampler2D _RampLight;
      sampler2D _RampDark;
	  half _Brightness;
	  half _Luma;
	  half _Fade;
	  half _Trans;
	  float4 _Color;
	  float3 _TransColor;
	  float4 _LightVector;
	  
	  
	  
      half4 LightingRampLight (SurfaceOutput s, half3 lightDir, half3 viewDir, fixed atten) {
        float light = dot(s.Normal,_LightVector);
		float rim = dot(s.Normal,viewDir);
		
		float diff = (light*.5)+.5;

		float2 brdfdiff = float2(rim, diff);
		float3 BRDFDark = tex2D(_RampDark, brdfdiff.xy).rgb;
		float3 BRDFLight = tex2D(_RampLight, brdfdiff.xy).rgb;
		
		half3 BRDFColor = (s.Albedo*_Color);

          half4 c;
          c.rgb =BRDFColor*lerp(BRDFDark*_Fade,BRDFLight*_Fade,_Trans);
          c.a = _Color.a;
          return c;
      }

      

	  
      void surf (Input IN, inout SurfaceOutput o) {
      
      

      float4 maintex = tex2D (_MainTex, IN.uv_MainTex);
      float4 alttex = tex2D (_AltTex, IN.uv_MainTex);
      half transtex = tex2D (_TransTex, IN.uv_TransTex).r;
      half transwipe = ceil(_Trans+(transtex*-1));
      half transinv = transwipe*-1+1;
      half3 transcolor = lerp(alttex,_TransColor,transtex*(_Trans*2));
      half3 transdarken = lerp(0,maintex,saturate((transtex*-1)+(_Trans*2)));

		  o.Albedo = lerp(transdarken,transcolor,transinv);
		  o.Emission = lerp((maintex.a*transdarken),(alttex.a*transcolor),transinv)*_Luma;

		 
		
      }
      ENDCG
    }
    Fallback "Diffuse"
  }
