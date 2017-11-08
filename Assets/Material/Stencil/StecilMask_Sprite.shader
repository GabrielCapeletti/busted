// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Stencil/Masks/StencilMask_Sprite"
{
	Properties {
        _MainTex ("_MainTex", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

	SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry-100"}
		ColorMask 0
		ZWrite off
		Stencil 
		{
			Ref 1
			Comp always
			Pass replace
		}
		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			half _Cutoff;
			sampler2D _MainTex;
			half4 _MainTex_ST;

			struct appdata 
			{
				float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
			};
			
			v2f vert(appdata v) 
			{
				v2f o;
				o.uv = v.texcoord0;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			half4 frag(v2f i) : COLOR 
			{
				half4 col = tex2D(_MainTex, i.uv);
				
				if (col.a < _Cutoff) discard;

				return col;
			}
		ENDCG
		}
	}
}