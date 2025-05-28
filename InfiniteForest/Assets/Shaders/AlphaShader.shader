Shader "AlphaShader"
{
    Properties
    {
        _MainTex("Albedo Texture", 2D) = "white" {}
        _BGColor("Background Color", Color) = (1,1,1,1)
        _FadeStartDistance("FadeStartDistance", Float) = 5
        _FadeCompleteDistance("FadeCompleteDistance", Float) = 10
        _Steps("Steps", Float) = 10
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        LOD 100

        ZWrite Off

        Pass
        {
            AlphaToMask On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BGColor;
            float _FadeStartDistance;
            float _FadeCompleteDistance;
            float _Steps;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float dist : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.dist = length(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                /*
                if (i.dist > _FadeCompleteDistance) {
                    col = _BGColor; // faded out. Opacity = 0
                }
                else if (i.dist > _FadeStartDistance) {
                    float mix = (_FadeCompleteDistance - i.dist) / (_FadeCompleteDistance - _FadeStartDistance);
                    col = (mix) * col + (1 - mix) * _BGColor;
                }
                */

                if (i.dist > _FadeCompleteDistance) {
                    col = _BGColor; // faded out. Opacity = 0
                }
                else if (i.dist > _FadeStartDistance) {
                    //float mix = (i.dist - _FadeStartDistance) / (_FadeCompleteDistance - _FadeStartDistance);
                    //if (fmod(i.vertex.x, mix * _Steps) >= 1 || fmod(i.vertex.y, mix * _Steps) >= 1) {
                    //    col = _BGColor;
                    //}
                    //fmod(i.vertex.x + i.vertex.y, 2) != 0 || 
                    if (fmod(i.vertex.x, 2) == 0 || fmod(i.vertex.y, 2) == 0){
                        col = _BGColor;
                    }
                }

                return col;
            }
            ENDCG
        }
    }
}
