// ---------------------------------------------------------------------
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// ---------------------------------------------------------------------

  Shader "Magic Leap/Unlit/AdditiveWidthMaskScroll" {
    Properties {
      
 
      _MainTex ("Texture", 2D) = "White" {}
      _MaskTex ("Mask", 2D) = "Black" {}
      _Multi ("Multiplier", Range(0.0,5)) = 0.0
      _ScrollSpeed ("Scroll",Range(-2,2)) = 2
      
      _Color1 ("Color1",Color)= (1,0,0,1)
      _Color2 ("Color2",Color)= (0,1,0,1)
      _Color3 ("Color3",Color)= (0,0,1,1)
      _Fade ("Fade", Range(0.0,2)) = 1
      _Edge ("Edge", Range(0.0,2)) = 1


      
    }
    SubShader {
      Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
      LOD 200
      ZWrite Off
      Lighting Off
      Cull back
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
          float2 uv2_MainTex;
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
      float _Edge;




      
      void surf (Input IN, inout SurfaceOutput o) {
      
          fixed2 scrolledUVR = IN.uv2_MainTex;
          fixed ScrollValueR = _ScrollSpeed;
          scrolledUVR += fixed2(0,ScrollValueR);
          half3 masktex = tex2D (_MaskTex,scrolledUVR);
          half mask = masktex.r;
          half edge = masktex.g*_Edge;
          
          half4 maintex =tex2D (_MainTex,IN.uv_MaskTex);

      
	      half3 maintexR = ceil(((maintex.a+edge)-1)+saturate(_Fade*(mask*-1+1)));
	      half3 effectcolor = lerp(_Color2,_Color1,maintex.a+edge);

	      half3 maincomp = lerp(maintex*_Multi,effectcolor,maintexR);

          o.Emission = lerp(maincomp,0,mask);
          
          
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }