Shader "UI/SDFCutout"
{
    Properties
    {
        _Color ("Overlay Color", Color) = (0,0,0,0.75)

        _HolePosition ("Hole Position", Vector) = (0.5,0.5,0,0)
        _HoleRadius ("Hole Radius", Float) = 0.15
        _EdgeSoftness ("Edge Softness", Float) = 0.02
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;

            float4 _HolePosition;
            float _HoleRadius;
            float _EdgeSoftness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.uv, _HolePosition.xy);

                float alpha = smoothstep(
                    _HoleRadius,
                    _HoleRadius + _EdgeSoftness,
                    dist);

                fixed4 col = _Color;
                col *= i.color;
                col.a *= alpha;

                return col;
            }

            ENDCG
        }
    }
}