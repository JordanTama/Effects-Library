Shader "Spells/Ignitor/CorePulse"
{
    Properties
    {
        [HDR] _Col ("Colour", Color) = (1, 1, 1, 1)
        _Noise ("Noise", 2D) = "white" {}
        _Amount ("Pulse Amount", Float) = 1
        _Frequency ("Pulse Frequency", Float) = 1
    }
    
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            
            #include "UnityCG.cginc"

            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                
                UNITY_FOG_COORDS(1)
            };

            
            float4 _Col;

            sampler2D _Noise;
            float4 _Noise_ST;

            float _Amount;
            float _Frequency;
            

            v2f vert (appdata v)
            {
                v2f o;
                
                o.uv = TRANSFORM_TEX(v.uv, _Noise);

                float2 uv = float2(_Time[1] * _Frequency, 0);
                float noise_sample = tex2Dlod(_Noise, float4(uv, 0, 0)).r;

                float3 normal = normalize(UnityObjectToWorldNormal(v.normal));
                float amount = _Amount * noise_sample;

                o.vertex = UnityObjectToClipPos(v.vertex - normal * amount);

                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = _Col;
                
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
