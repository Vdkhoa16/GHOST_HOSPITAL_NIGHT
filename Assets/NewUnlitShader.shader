Shader "Custom/OneWayMirror"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0,1)) = 0.5
        _ReflectColor ("Reflection Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            float4 _Color;
            float _Transparency;
            float4 _ReflectColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul((float3x3)unity_WorldToObject, v.normal));
                o.viewDir = normalize(UnityWorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Nếu hướng nhìn và hướng bề mặt là cùng chiều, kính sẽ trong suốt
                if (dot(i.normal, i.viewDir) > 0)
                {
                    fixed4 col = _Color;
                    col.a = _Transparency;
                    return col;
                }
                // Ngược lại, kính sẽ phản chiếu
                else
                {
                    return _ReflectColor;
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
