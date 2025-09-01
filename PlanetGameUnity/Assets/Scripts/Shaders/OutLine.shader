Shader "Custom/Outline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineThickness ("Thickness", Range(0.0, 0.1)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" } 
        LOD 100

        Pass
        {
            Name "Outline"
            Cull Front // 裏面を描くことで「外側の縁取り」になる
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineThickness;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v2f o;

                // 法線をワールド空間に変換
                float3 worldNormal = normalize(mul((float3x3) unity_WorldToObject, v.normal));
                // 頂点をワールド空間に変換
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                // 法線方向に押し出す
                worldPos.xyz += worldNormal * _OutlineThickness;
                // クリップ座標へ
                o.pos = mul(UNITY_MATRIX_VP, worldPos);

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
