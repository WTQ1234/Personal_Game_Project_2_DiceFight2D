Shader "UI/MaskedUIBlur" {
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255

		[HideInInspector]_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

		_Size("Size", Range(0, 50)) = 5
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
		{
			Name "FrontBlurHor"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			fixed4 _Color;
			float4 _ClipRect;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;

				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _Size;

			half4 GrabPixel(v2f IN) {
				if (_Size <= 1) {
					return tex2D(_MainTex, half2(IN.texcoord.x + _MainTex_TexelSize.x * 1 * _Size, IN.texcoord.y)) * 1;
				}
				else {
					int xScale = _Size;
					int yScale = _Size;
					half4 sum = half4(0,0,0,0);
					for (int i = 0; i < xScale; i++)
					{
						for (int j = 0; j < yScale; j++)
						{
							//if (i != xScale / 2 && j != yScale / 2)
							//{
							//	sum += tex2D(_MainTex, half2(
							//		IN.texcoord.x + (i - xScale / 2) * _MainTex_TexelSize.x,
							//		IN.texcoord.y + (j - yScale / 2) * _MainTex_TexelSize.y));
							//}
							sum += tex2D(_MainTex, half2(
								IN.texcoord.x + (i - xScale / 2) * _MainTex_TexelSize.x,
								IN.texcoord.y + (j - yScale / 2) * _MainTex_TexelSize.y));
						}
					}
					return sum / (xScale * yScale);
				}
			}

			fixed4 frag(v2f IN) : SV_Target {
				half4 sum = half4(0,0,0,0);
				sum += GrabPixel(IN);
				//for (int i = 0; i < 9; i++) {
				//	sum += GrabPixel(IN, 1.0 / 9, i - 4.0);
				//	//sum += GrabPixely(IN, 1.0 / 9, i - 4.0);
				//}

				sum = sum * IN.color;
				sum.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				#ifdef UNITY_UI_ALPHACLIP
				clip(sum.a - 0.001);
				#endif
				return sum;
			}
		ENDCG
		}
	}
}