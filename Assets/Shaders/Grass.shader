// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Grass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Lut ("Lut", 2D) = "white" {}
		_Paper("Paper", 2D) = "white" {}
		//_Fill ("Fill", Float) = 1
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
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 normal :TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _Lut;
			sampler2D _Paper;
			float _Fill;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = mul(unity_ObjectToWorld,v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float3 ogWorld = i.worldPos;
				if(abs(dot(float3(0,1,0),i.normal))<0.4){
					i.worldPos.xyz = i.worldPos.xzy;
				}
                fixed4 col = tex2D(_MainTex, i.worldPos.xz*0.25+_Time.x);
				fixed4 final = tex2D(_Lut,float2(col.x,0.5));
				float4 shadow = tex2D(_MainTex, i.worldPos.xz*0.05+_Time.x*0.1);
				float4 cutoff = tex2D(_MainTex, i.worldPos.xz*0.1+_Time.x*1);
				float4 paper = tex2D(_Paper, i.worldPos.xz*0.5+100);
					


				float thresh = max(
				smoothstep(4,6,abs(ogWorld.z)+(cutoff*_Fill)),
				smoothstep(9,11,abs(ogWorld.x)+(cutoff*_Fill)));
				shadow = smoothstep(0.4,0.65,shadow);
				final = 1-pow(1-final,1.5);
				//final = lerp(final,fixed4(1,1,1,1)*0.9,thresh);
                return lerp(final*(shadow*0.1+0.9),paper,thresh*0.6+0.4);
            }
            ENDCG
        }
    }
}
