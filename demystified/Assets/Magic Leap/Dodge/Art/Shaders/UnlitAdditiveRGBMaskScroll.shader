// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

  Shader "Magic Leap/Unlit/AdditiveRGBMaskScroll" {
    Properties {
      
 
      _MainTex ("Texture", 2D) = "White" {}
      _MaskTex ("Mask", 2D) = "Black" {}
      _Multi ("Multiplier", Range(0.0,5)) = 0.0
      _ScrollSpeed ("Scroll",Range(-25,25)) = 2
      
      _Color1 ("Color1",Color)= (1,0,0,1)
      _Color2 ("Color2",Color)= (0,1,0,1)
      _Color3 ("Color3",Color)= (0,0,1,1)
      _Fade ("Fade", Range(0.0,1)) = 1


      
    }
    SubShader {
      Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
      LOD 200
      ZWrite Off
      Lighting Off
      Cull Off
      ZTest LEqual
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
          float2 uv_MaskTex;
      
      };
      
      sampler2D _MainTex;
      sampler2D _MaskTex;
      float _Multi;
      fixed _ScrollSpeed;
      half3 _Color1;
      half3 _Color2;
      half3 _Color3;
      float _Fade;




      
      void surf (Input IN, inout SurfaceOutput o) {
      
          fixed2 scrolledUVR = IN.uv_MainTex;

          fixed ScrollValueR = _ScrollSpeed;
          scrolledUVR += fixed2(0,ScrollValueR);
          
	      half3 maintexR = tex2D (_MainTex, scrolledUVR)*_Color1;
	      float mask = tex2D (_MaskTex, IN.uv_MaskTex).r;
	      float frame = tex2D (_MaskTex, IN.uv_MaskTex).g*.4;

	      
	      half3 maincomp = _Color2+(maintexR*_Fade);

          o.Emission = maincomp*_Multi*mask;

      }
      ENDCG
    } 
    Fallback "Diffuse"
  }