Shader "Spells/Ignitor/Halo"
{
    Properties
    {
        [HDR] _Col ("Color", Color) = (1, 1, 1, 1)

		_DisplacementTex ("Displacement Texture", 2D) = "white" {}
		_Displacement ("Displacement Amount", Float) = 1
        
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _Dissolve ("Dissolve Amount", Range(0, 1)) = 0.5
        
        _Size ("Size", Range(0, 1)) = 0.5
        _Weight ("Weight", Range(0, 1)) = 0.1
        
        _Smoothness ("Smoothness", Range(0, 1)) = 0.1
    }
    
    SubShader
    {
        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
                float2 displacement_uv : TEXCOORD1;
				float2 dissolve_uv : TEXCOORD2;
                
                UNITY_FOG_COORDS(3)
            };


            float4 _Col;

			sampler2D _DisplacementTex;
            float4 _DisplacementTex_ST;

			float _Displacement;

            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;

            float _Dissolve;

            float _Size;
            float _Weight;

            float _Smoothness;


            float2 get_polar_uv(float2 uv)
            {
				float2 delta = uv - float2(0.5, 0.5);
				float radius = length(delta) * 2;
				float angle = atan2(delta.x, delta.y) / 6.28;
                return float2(radius, angle);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.displacement_uv = TRANSFORM_TEX(v.uv, _DisplacementTex);
                o.dissolve_uv = TRANSFORM_TEX(v.uv, _DissolveTex);
                
                UNITY_TRANSFER_FOG(o, o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				// Displacement
				const float4 displacement_sample = tex2D(_DisplacementTex, i.displacement_uv);
				const float displacement = _Displacement;
				const float2 displacement_value = (displacement_sample.rg * 2.0 - 1.0) * displacement;

				// Halo mask
				const float time = _Size;
				const float weight = _Weight * 0.5;
				const float half_weight = weight * 0.5;

				const float centre_radius = lerp(0.0 - weight, 0.5 - weight - displacement, time);
				const float radius = distance(float2(0.5, 0.5), i.uv + displacement_value);

				float halo_value = 1.0 - saturate(abs(radius - centre_radius) / weight);
				
				// Dissolve mask
				const float dissolve_amount = ((1.0 - _Dissolve) * 2.0) - 1.0;
				const float4 dissolve_sample = tex2D(_DissolveTex, i.dissolve_uv + displacement_value);
				float dissolve_value = saturate(dissolve_sample.r + dissolve_amount);

				// Combining masks
				const float smoothness = _Smoothness;
				const float output_value = smoothstep(0.0, smoothness, halo_value * dissolve_value);

                float4 col = float4(_Col.rgb, output_value);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
