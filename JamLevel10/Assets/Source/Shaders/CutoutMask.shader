Shader "UI/SDFCutoutURP"
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
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Pass
        {
            Name "Forward"

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            half4 _Color;
            float4 _HolePosition;
            float _HoleRadius;
            float _EdgeSoftness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float dist = distance(IN.uv, _HolePosition.xy);

                float alpha = smoothstep(
                    _HoleRadius,
                    _HoleRadius + _EdgeSoftness,
                    dist);

                half4 col = _Color;
                col *= IN.color;
                col.a *= alpha;

                return col;
            }

            ENDHLSL
        }
    }
}