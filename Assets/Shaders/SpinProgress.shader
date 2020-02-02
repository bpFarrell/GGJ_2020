Shader "Unlit/SpinProgress"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_T("t",Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _T;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float t = _T*3.14529*3;
				t+=6;
				i.uv = i.uv*2-1;
				float d = length(i.uv);
				float f = smoothstep(0.13,0.05,abs(d -0.7));
				f = min(f,atan2(i.uv.x,i.uv.y)+t);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				if(f<0.5){
					discard;
				}
                return col*f;
            }
            ENDCG
        }
    }
}
