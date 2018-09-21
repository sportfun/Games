// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ColorBars"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_PrimaryColor("Primary Color", Color) = (0,1,0,1)
		_SecondaryColor("Secondary Color", Color) = (1,0,0,1)
		_Value("Value", Range( 0 , 1)) = 1
		_Position("Position", Range( 0.1 , 0.9)) = 0.9
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

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
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _SecondaryColor;
			uniform float4 _PrimaryColor;
			uniform float _Value;
			uniform float _Position;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode22 = tex2D( _MainTex, uv_MainTex );
				float4 lerpResult18 = lerp( _SecondaryColor , _PrimaryColor , step( ( 1.0 - _Value ) , ( 1.0 - pow( abs( (( _Position + 0.5 ) + (IN.texcoord.xy.y - 0.0) * (( _Position + -1.5 ) - ( _Position + 0.5 )) / (1.0 - 0.0)) ) , 0.5 ) ) ));
				
				half4 color = ( tex2DNode22 * ( tex2DNode22.a * lerpResult18 ) );
				
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=15600
421;92;873;706;2358.668;1170.775;2.936389;True;False
Node;AmplifyShaderEditor.RangedFloatNode;56;-2633.592,378.6465;Float;False;Property;_Position;Position;3;0;Create;True;0;0;False;0;0.9;0.1;0.1;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-2293.433,596.8806;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;9;-2304,128;Float;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-2313.948,338.5223;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;39;-2046.348,128;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;42;-1664,128;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;43;-1408,128;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1536,-128;Float;False;Property;_Value;Value;2;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;50;-896,-768;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;44;-1152,128;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;-1152,-128;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-896,-384;Float;False;Property;_PrimaryColor;Primary Color;0;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2;-896,-576;Float;False;Property;_SecondaryColor;Secondary Color;1;0;Create;True;0;0;False;0;1,0,0,1;1,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-640,-768;Float;True;Property;_MainTex;Main Texture;3;1;[HideInInspector];Create;False;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;19;-896,0;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;-512,-512;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;53;-304,-560;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-256,-512;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;54;-80,-672;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;0,-512;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;256,-512;Float;False;True;2;Float;ASEMaterialInspector;0;4;ColorBars;5056123faa0c79b47ab6ad7e8bf059a4;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;False;-1;False;True;5;Queue=Transparent;IgnoreProjector=True;RenderType=Transparent;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;58;0;56;0
WireConnection;57;0;56;0
WireConnection;39;0;9;2
WireConnection;39;3;58;0
WireConnection;39;4;57;0
WireConnection;42;0;39;0
WireConnection;43;0;42;0
WireConnection;44;0;43;0
WireConnection;49;0;5;0
WireConnection;22;0;50;0
WireConnection;19;0;49;0
WireConnection;19;1;44;0
WireConnection;18;0;2;0
WireConnection;18;1;4;0
WireConnection;18;2;19;0
WireConnection;53;0;22;4
WireConnection;24;0;53;0
WireConnection;24;1;18;0
WireConnection;54;0;22;0
WireConnection;25;0;54;0
WireConnection;25;1;24;0
WireConnection;1;0;25;0
ASEEND*/
//CHKSM=AA762C83F7437AECD2CF44D9CF422B22CCED788C