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
            Cull Front // ���ʂ�`�����ƂŁu�O���̉����v�ɂȂ�
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

                // �@�������[���h��Ԃɕϊ�
                float3 worldNormal = normalize(mul((float3x3) unity_WorldToObject, v.normal));
                // ���_�����[���h��Ԃɕϊ�
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                // �@�������ɉ����o��
                worldPos.xyz += worldNormal * _OutlineThickness;
                // �N���b�v���W��
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
