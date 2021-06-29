Shader "Spells/Ignitor/Trail"
{
    Properties
    {
		[HDR] _Col ("Colour", Color) = (1, 1, 1, 1)

		_Noise ("Texture", 2D) = "white" {}
		_Amount("Dissolve Amount", Range(0, 1)) = 0.5
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
				float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
                float2 noise_uv : TEXCOORD1;
				UNITY_FOG_COORDS(2)
            };

			float4 _Col;
            
			sampler2D _Noise;
            float4 _Noise_ST;
			float _Amount;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.uv;
                o.noise_uv = TRANSFORM_TEX(v.uv, _Noise);

                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float amount = (1.0 - _Amount) * 2.0 - 1.0;

				// STEP 1. Calculate noise dissolve amount.
                const float4 noise_sample = tex2D(_Noise, i.noise_uv);
				float noise_value = saturate(noise_sample.r + amount);

				// STEP 2. Calculate width dissolve amount.
				float width_value = 1.0 - abs((i.uv.y - 0.5) * 2.0);
				width_value = saturate(width_value + amount);

				// STEP 3. Combining and smoothness processing.
				float dissolve_value = noise_value * width_value;

				const float smoothness = 0.01;
				const float step_min = lerp(0, 1.0 - smoothness, _Amount);
				const float step_max = lerp(smoothness, 1.0, _Amount);

				dissolve_value = smoothstep(step_min, step_max, dissolve_value);
				//dissolve_value = saturate(dissolve_value + amount);

				float4 col = float4(_Col.rgb, dissolve_value);
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
