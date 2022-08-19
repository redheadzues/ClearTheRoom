Shader "Unlit/ClearableShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask("Mask", 2D) = "white"{ }
        _Radius("Radius", float) = 100
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "IgnoreProjector"="True" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Zwrite off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

 			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

            sampler2D _MainTex;
            sampler2D _Mask;
            float4 _MainTex_ST;
            float _Radius;
            float2 _PointOffset;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) :  SV_Target
            {
                float4 mainTexColor = tex2D(_MainTex, i.uv);
                float4 maskColor = tex2D(_Mask, i.uv);
                float2 uv = i.uv - 0.5 + _PointOffset;
                float distance = sqrt(pow((uv.x), 2) + pow((uv.y), 2))/ _Radius;

                if(distance < _Radius)
                {
                    mainTexColor.a = 0;
                }

                return mainTexColor;
            }
            ENDCG
        }
    }
}
