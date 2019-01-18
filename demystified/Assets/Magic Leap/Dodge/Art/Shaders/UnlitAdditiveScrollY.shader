// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

  Shader "Magic Leap/Unlit/AdditiveScrollY" {
    Properties {
      
 
      _MainTex ("Texture", 2D) = "White" {}
      _Multi ("Multiplier", Range(0.0,5)) = 0.0
      _ScrollSpeed ("Scroll",Range(-50,50)) = 20
      
      _Color1 ("Color1",Color)= (1,0,0,1)
 


      
    }
    SubShader {
      Tags { "Queue"="Transparent" "RenderType" = "Transparent" }
      LOD 200
      ZWrite Off
      Lighting Off
      Cull back
      ZTest Always
      Blend One One 
      Fog { Mode Off}
      CGPROGRAM
     #pragma surface surf Additive  halfasview novertexlights exclude_path:prepass noambient noforwardadd nolightmap nodirlightmap

      half4 LightingAdditive (SurfaceOutput s, half3 lightDir, half3 viewDir) {
		half3 h = normalize (lightDir + viewDir);


          half4 c;
          c.rgb = s.Albedo;
          c.a = s.Alpha;
          return c;
      }
      struct Input {
          float2 uv_MainTex;

      };
      
      sampler2D _MainTex;
      float _Multi;
      fixed _ScrollSpeed;
      half3 _Color1;

      
      void surf (Input IN, inout SurfaceOutput o) {
      
          fixed2 scrolledUVR = IN.uv_MainTex;
          fixed ScrollValueR = _ScrollSpeed * _Time;
          scrolledUVR += fixed2(ScrollValueR,0);

          
          half4 maintex =tex2D (_MainTex, scrolledUVR);


          o.Emission = (maintex*_Color1)*_Multi;
  
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }