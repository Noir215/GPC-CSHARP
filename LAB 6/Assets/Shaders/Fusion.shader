Shader "Custom/Fusion"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		_SecondTex ("Second Tex (RGB)", 2D) = "white" {}
		_ColorEffect ("Color Effect", Color) = (1, 0, 0, 1)  // Rojo
		_ValorUmbral ("Valor Umbral", Float) = 1.0
		_ValorRango ("Valor Rango", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _SecondTex;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_SecondTex;
			half3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
		half _ValorUmbral;
		half _ValorRango;

        fixed4 _Color;
		fixed4 _ColorEffect;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

			fixed4 c2 = tex2D(_SecondTex, IN.uv_SecondTex);

			// Albedo de interpolacion entre c y c2
			if (IN.worldPos.y > _ValorUmbral) {
				o.Albedo = c.rgb;
			}
			else if (IN.worldPos.y > _ValorUmbral - _ValorRango && IN.worldPos.y < _ValorUmbral) {
				o.Albedo = lerp(c.rgb, c2.rgb, 0.5);
				o.Emission = _ColorEffect * abs((IN.worldPos.y - _ValorUmbral));
			}
			else {
				o.Albedo = c2.rgb;
			}
        }
        ENDCG
    }
    FallBack "Diffuse"
}
