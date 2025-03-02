// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Davis3D/LowPolyOceanPack2/Lowpoly Ocean Leaf"
{
	Properties
	{
		_ColorA("Color A", Color) = (0.4705882,0.8784314,0.9098039,1)
		_ColorB("Color B", Color) = (0.1686275,0.007843138,0,1)
		_GradientPower("Gradient Power", Float) = 1
		_ColorMultiplyA("Color Multiply A", Float) = 1
		_ColorMultiplyA1("Color Multiply A", Float) = 5
		_VariationHue("Variation Hue", Float) = 0.3
		_VariationBrightness("Variation Brightness", Float) = 0.3
		_SpecColor("Specular Color",Color)=(1,1,1,1)
		_Metallic("Metallic", Range( 0.01 , 1)) = 0.01
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		_GlowA("Glow A", Float) = 0.05
		_GlowB("Glow B", Float) = 0.2
		_FresnelScale("Fresnel Scale", Float) = 1
		_FresnelPower("Fresnel Power", Float) = 5
		[Header(FakeLighting)][Space][Toggle(_ENABLEFAKELIGHTING_ON)] _EnableFakeLighting("Enable FakeLighting", Float) = 0
		_FakeLighting_Glow_A("Glow A", Float) = 0
		_FakeLighting_Glow_B("Glow B", Float) = 0
		_FakeLightingGradientPower("Gradient Power", Float) = 1
		[Toggle(_ENABLEWIND_ON)] _EnableWind("Enable Wind", Float) = 0
		_WindIntensity("Wind Intensity", Float) = 0.5
		_WindSpeed("Wind Speed", Float) = 0.5
		_GrassWindGradient("Grass Wind Gradient", Float) = 1
		[Header(Caustic)][Space][Toggle(_ENABLECAUSTIC_ON)] _EnableCaustic("Enable Caustic", Float) = 0
		_CausticColor("CausticColor", Color) = (0.5960785,0.8666667,1,1)
		_CausticTexture("CausticTexture", 2D) = "white" {}
		_CausticIntensity("Caustic Intensity", Float) = 1
		_SubCausticIntensity("SubCaustic Intensity", Float) = 0.75
		_Caustic1Scale("Caustic 1 Scale", Float) = 2
		_Caustic2Scale("Caustic 2 Scale", Float) = 2
		_Caustic3Scale("Caustic 3 Scale", Float) = 2
		_CausticMasterScale("Caustic Master Scale", Float) = 0.05
		_Caustic1Speed("Caustic 1 Speed", Float) = 0.2
		_Caustic2Speed("Caustic 2 Speed", Float) = 0.05
		_Caustic3Speed("Caustic 3 Speed", Float) = -0.16
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _ENABLEWIND_ON
		#pragma shader_feature_local _ENABLEFAKELIGHTING_ON
		#pragma shader_feature_local _ENABLECAUSTIC_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv2_texcoord2;
		};

		uniform float _WindSpeed;
		uniform float _WindIntensity;
		uniform float _GrassWindGradient;
		uniform float _VariationHue;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform float _GradientPower;
		uniform float _VariationBrightness;
		uniform float _ColorMultiplyA;
		uniform float _ColorMultiplyA1;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _GlowA;
		uniform float _GlowB;
		uniform float _FakeLighting_Glow_A;
		uniform float _FakeLighting_Glow_B;
		uniform float _FakeLightingGradientPower;
		uniform sampler2D _CausticTexture;
		uniform float _Caustic1Scale;
		uniform float _CausticMasterScale;
		uniform float _Caustic1Speed;
		uniform float _Caustic2Scale;
		uniform float _Caustic2Speed;
		uniform float _SubCausticIntensity;
		uniform float _Caustic3Scale;
		uniform float _Caustic3Speed;
		uniform float4 _CausticColor;
		uniform float _CausticIntensity;
		uniform float _Metallic;
		uniform float _Smoothness;


		float3 WorldToAbsoluteWorld3_g599( float3 In )
		{
			#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
			    In += _WorldSpaceCameraPos.xyz;
			#endif
			return In;
		}


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		inline float4 TriplanarSampling1_g604( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling3_g604( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling6_g604( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 _0101 = float4(0,1,0,1);
			float3 appendResult13_g598 = (float3(_0101.x , _0101.y , _0101.z));
			float3 normalizeResult14_g598 = normalize( appendResult13_g598 );
			float3 temp_cast_0 = (3.0).xxx;
			float temp_output_19_0_g598 = ( _0101.w * ( ( _Time.y * _WindSpeed ) * -0.5 ) );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 In3_g599 = ase_worldPos;
			float3 localWorldToAbsoluteWorld3_g599 = WorldToAbsoluteWorld3_g599( In3_g599 );
			float3 temp_output_62_0_g598 = localWorldToAbsoluteWorld3_g599;
			float3 temp_output_45_0_g598 = abs( ( ( frac( ( ( ( normalizeResult14_g598 * temp_output_19_0_g598 ) + ( temp_output_62_0_g598 / 10.24 ) ) + 0.5 ) ) * 2.0 ) + -1.0 ) );
			float dotResult58_g598 = dot( normalizeResult14_g598 , ( ( ( temp_cast_0 - ( temp_output_45_0_g598 * 2.0 ) ) * temp_output_45_0_g598 ) * temp_output_45_0_g598 ) );
			float3 temp_cast_1 = (3.0).xxx;
			float3 temp_output_46_0_g598 = abs( ( ( frac( ( ( temp_output_19_0_g598 + ( temp_output_62_0_g598 / 2.0 ) ) + 0.5 ) ) * 2.0 ) + -1.0 ) );
			float3 temp_cast_2 = (0.0).xxx;
			float3 temp_output_8_0_g598 = float3( 0,0,0 );
			float3 rotatedValue6_g598 = RotateAroundAxis( ( float3(0,0,-10) + temp_output_8_0_g598 ), temp_output_8_0_g598, cross( normalizeResult14_g598 , float3(0,0,1) ), ( dotResult58_g598 + distance( ( ( ( temp_cast_1 - ( temp_output_46_0_g598 * 2.0 ) ) * temp_output_46_0_g598 ) * temp_output_46_0_g598 ) , temp_cast_2 ) ) );
			float lerpResult88 = lerp( 0.0 , ( 1.0 - v.texcoord2.xy.y ) , _GrassWindGradient);
			float3 lerpResult87 = lerp( ( ( ( rotatedValue6_g598 * 1.0 ) * _WindIntensity ) + temp_output_8_0_g598 ) , float3( 0,0,0 ) , lerpResult88);
			#ifdef _ENABLEWIND_ON
				float3 staticSwitch86 = lerpResult87;
			#else
				float3 staticSwitch86 = float3( 0,0,0 );
			#endif
			float3 WindOffset93 = staticSwitch86;
			v.vertex.xyz += WindOffset93;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Normal = float3(0,0,1);
			float4 color5_g596 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 normalizeResult6_g596 = normalize( color5_g596 );
			float4 transform36 = mul(unity_WorldToObject,float4( 0,0,0,1 ));
			float2 appendResult37 = (float2(transform36.x , transform36.z));
			float dotResult4_g597 = dot( appendResult37 , float2( 12.9898,78.233 ) );
			float lerpResult10_g597 = lerp( 0.0 , 1.0 , frac( ( sin( dotResult4_g597 ) * 43758.55 ) ));
			float WSRandomizer53 = lerpResult10_g597;
			float lerpResult40 = lerp( ( -0.1 * _VariationHue ) , ( _VariationHue * 0.1 ) , WSRandomizer53);
			float VariationHue46 = lerpResult40;
			float3 temp_cast_1 = (0.0).xxx;
			float3 temp_output_3_0_g596 = _ColorA.rgb;
			float3 rotatedValue2_g596 = RotateAroundAxis( temp_cast_1, temp_output_3_0_g596, normalizeResult6_g596.rgb, VariationHue46 );
			float4 color5_g595 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
			float4 normalizeResult6_g595 = normalize( color5_g595 );
			float3 temp_cast_4 = (0.0).xxx;
			float3 temp_output_3_0_g595 = _ColorB.rgb;
			float3 rotatedValue2_g595 = RotateAroundAxis( temp_cast_4, temp_output_3_0_g595, normalizeResult6_g595.rgb, VariationHue46 );
			float clampResult32 = clamp( pow( ( 1.0 - i.uv_texcoord.y ) , _GradientPower ) , 0.0 , 1.0 );
			float3 lerpResult31 = lerp( ( rotatedValue2_g596 + temp_output_3_0_g596 ) , ( rotatedValue2_g595 + temp_output_3_0_g595 ) , clampResult32);
			float lerpResult62 = lerp( ( ( -0.1 * _VariationBrightness ) + 1.0 ) , ( ( _VariationBrightness * 0.1 ) + 1.0 ) , WSRandomizer53);
			float VariationBrightness63 = lerpResult62;
			float3 temp_output_28_0 = ( lerpResult31 * VariationBrightness63 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV16 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode16 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV16, _FresnelPower ) );
			float Fresnel22 = fresnelNode16;
			float3 lerpResult19 = lerp( ( temp_output_28_0 * _ColorMultiplyA ) , ( _ColorMultiplyA1 * temp_output_28_0 ) , Fresnel22);
			o.Albedo = lerpResult19;
			float lerpResult5 = lerp( _FakeLighting_Glow_A , _FakeLighting_Glow_B , ( 1.0 - pow( ( 1.0 - i.uv2_texcoord2.y ) , _FakeLightingGradientPower ) ));
			#ifdef _ENABLEFAKELIGHTING_ON
				float staticSwitch8 = lerpResult5;
			#else
				float staticSwitch8 = 0.0;
			#endif
			float3 lerpResult12 = lerp( ( _GlowA * temp_output_28_0 ) , ( temp_output_28_0 * _GlowB ) , ( Fresnel22 + staticSwitch8 ));
			float2 temp_cast_6 = (( _Caustic1Scale * _CausticMasterScale )).xx;
			float temp_output_2_0_g607 = ( _Time.y * _Caustic1Speed );
			float4 triplanar1_g604 = TriplanarSampling1_g604( _CausticTexture, ( ase_worldPos + temp_output_2_0_g607 ), ase_worldNormal, 1.0, temp_cast_6, 1.0, 0 );
			float2 temp_cast_7 = (( _Caustic2Scale * _CausticMasterScale )).xx;
			float temp_output_2_0_g605 = ( _Time.y * _Caustic2Speed );
			float4 triplanar3_g604 = TriplanarSampling3_g604( _CausticTexture, ( ase_worldPos + temp_output_2_0_g605 ), ase_worldNormal, 1.0, temp_cast_7, 1.0, 0 );
			float2 temp_cast_8 = (( _Caustic3Scale * _CausticMasterScale )).xx;
			float temp_output_2_0_g606 = ( _Time.y * _Caustic3Speed );
			float4 triplanar6_g604 = TriplanarSampling6_g604( _CausticTexture, ( ase_worldPos + temp_output_2_0_g606 ), ase_worldNormal, 1.0, temp_cast_8, 1.0, 0 );
			#ifdef _ENABLECAUSTIC_ON
				float3 staticSwitch97 = ( (( ( ( ( triplanar1_g604.x + triplanar3_g604.x ) * _SubCausticIntensity ) * triplanar6_g604.x ) * _CausticColor )).rgb * _CausticIntensity );
			#else
				float3 staticSwitch97 = float3( 0,0,0 );
			#endif
			o.Emission = ( lerpResult12 + staticSwitch97 );
			o.Specular = _Metallic;
			o.Gloss = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf BlinnPhong keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.zw = customInputData.uv2_texcoord2;
				o.customPack1.zw = v.texcoord1;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_texcoord2 = IN.customPack1.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19108
Node;AmplifyShaderEditor.CommentaryNode;92;-775.2095,1199.169;Inherit;False;1126.871;482.0327;Wind;11;90;93;87;88;89;86;82;85;84;83;95;;0.7998468,1,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;54;-974.087,942.5073;Inherit;False;770.9323;257;Randomizer;4;38;53;37;36;;0.9339623,0.03083839,0.03083839,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;47;-1658.806,1229.602;Inherit;False;777.1099;336.7278;VariationHue;6;55;40;46;44;43;42;;0,1,0.5681005,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;26;-1658.412,941.7548;Inherit;False;678.7158;280.8282;Fresnel;4;22;25;24;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;9;-1763.063,562.1355;Inherit;False;1126.269;372.8481;FakeLighting;9;1;81;7;6;8;5;4;3;2;;0.6745283,1,0.9293792,1;0;0
Node;AmplifyShaderEditor.PowerNode;2;-1379.674,754.0192;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1574.41,818.9828;Inherit;False;Property;_FakeLightingGradientPower;Gradient Power;17;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;4;-1233.167,755.3764;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-1080.749,672.5133;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;8;-929.7964,643.3068;Inherit;False;Property;_EnableFakeLighting;Enable FakeLighting;14;0;Create;True;0;0;0;False;2;Header(FakeLighting);Space;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-623.1237,473.5016;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-292.3305,135.4649;Inherit;False;Property;_Smoothness;Smoothness;9;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;12;-477.5819,229.4801;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;19;-241.5964,-107.4255;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-630.5254,253.5748;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-402.4536,-85.38615;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;-444.694,6.603439;Inherit;False;22;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-1203.696,991.7545;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1608.412,1106.582;Inherit;False;Property;_FresnelPower;Fresnel Power;13;0;Create;True;0;0;0;False;0;False;5;3.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1602.412,1036.582;Inherit;False;Property;_FresnelScale;Fresnel Scale;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;16;-1431.885,996.8606;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-795.3143,468.6557;Inherit;False;22;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-630,166;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-789.6447,159.4998;Inherit;False;Property;_GlowA;Glow A;10;0;Create;True;0;0;0;False;0;False;0.05;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-787.4932,273.1518;Inherit;False;Property;_GlowB;Glow B;11;0;Create;True;0;0;0;False;0;False;0.2;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1234.014,612.1349;Inherit;False;Property;_FakeLighting_Glow_A;Glow A;15;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1235.014,682.1347;Inherit;False;Property;_FakeLighting_Glow_B;Glow B;16;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;31;-1310.08,-297.5212;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;32;-1499.08,-219.5212;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;33;-1647.08,-219.5212;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1829.865,-148.7023;Inherit;False;Property;_GradientPower;Gradient Power;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;51;-1857.723,-407.6857;Inherit;False;HueShift;-1;;595;3748b6194161e1143a6905ade0b83f9a;0;2;1;FLOAT;0;False;3;FLOAT3;0,1,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;50;-1857.723,-567.6857;Inherit;False;HueShift;-1;;596;3748b6194161e1143a6905ade0b83f9a;0;2;1;FLOAT;0;False;3;FLOAT3;0,1,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;48;-2097.793,-617.3679;Inherit;False;Property;_ColorA;Color A;0;0;Create;True;0;0;0;False;0;False;0.4705882,0.8784314,0.9098039,1;0,0.8823529,0.9098039,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;38;-601.4377,1029.538;Inherit;False;Random Range;-1;;597;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;-429.1553,1024.356;Inherit;False;WSRandomizer;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;37;-735.6321,1028.293;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;36;-924.087,992.5078;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;52;-2071.723,-452.6857;Inherit;False;46;VariationHue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;49;-2091.724,-386.6857;Inherit;False;Property;_ColorB;Color B;1;0;Create;True;0;0;0;False;0;False;0.1686275,0.007843138,0,1;0,0.03921569,0.1686274,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;57;-1659.839,1573.119;Inherit;False;872.2682;340.2032;VariationBrightness;8;63;62;65;64;61;60;59;58;;1,0,0.6716323,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1440.643,1623.119;Inherit;False;2;2;0;FLOAT;-0.1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1441.643,1724.119;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-1512.82,1817.07;Inherit;False;53;WSRandomizer;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-1511.786,1473.554;Inherit;False;53;WSRandomizer;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;40;-1268.673,1338.561;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;-1118.515,1334.547;Inherit;False;VariationHue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1439.885,1271.164;Inherit;False;2;2;0;FLOAT;-0.1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1618.463,1330.164;Inherit;False;Property;_VariationHue;Variation Hue;5;0;Create;True;0;0;0;False;0;False;0.3;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1439.464,1372.164;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-1296.592,1625.078;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-1296.806,1726.071;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;62;-1130.366,1721.765;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;63;-987.2093,1716.751;Inherit;False;VariationBrightness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1089.766,-112.5082;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1649.086,1680.764;Inherit;False;Property;_VariationBrightness;Variation Brightness;6;0;Create;True;0;0;0;False;0;False;0.3;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-401.9282,-172.961;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-609.7719,-155.1454;Inherit;False;Property;_ColorMultiplyA;Color Multiply A;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-610.1831,-84.6496;Inherit;False;Property;_ColorMultiplyA1;Color Multiply A;4;0;Create;True;0;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;-2104.08,-220.5212;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;80;-1808.154,-220.7216;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;81;-1528.488,752.1072;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1748.84,704.559;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;66;-1341.504,-42.28196;Inherit;False;63;VariationBrightness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-669.3499,1504.209;Inherit;False;Constant;_Float1;Float 1;18;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-708.3499,1433.209;Inherit;False;Property;_WindIntensity;Wind Intensity;19;0;Create;True;0;0;0;False;0;False;0.5;0.015;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-700.3499,1574.209;Inherit;False;Property;_WindSpeed;Wind Speed;20;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;82;-514.2936,1486.841;Inherit;False;SimpleGrassWind;-1;;598;2f5588d2087aaab40a4d9a662ffe0ccd;0;4;5;FLOAT;0;False;7;FLOAT;0;False;23;FLOAT;1;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;86;-56.02777,1372.236;Inherit;False;Property;_EnableWind;Enable Wind;18;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-750.0339,1363.626;Inherit;False;Property;_GrassWindGradient;Grass Wind Gradient;21;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;87;-220.7028,1402.364;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;153.4088,1372.018;Inherit;False;WindOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;90;-752.7972,1250.169;Inherit;False;2;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;88;-399.6082,1349.91;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;95;-547.7761,1300.668;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;124.0903,-4.834686;Float;False;True;-1;2;ASEMaterialInspector;0;0;BlinnPhong;Davis3D/LowPolyOceanPack2/Lowpoly Ocean Leaf;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;7;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-69.05416,252.6425;Inherit;False;93;WindOffset;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;-286.828,229.3859;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;97;-551.6275,345.4857;Inherit;False;Property;_EnableCaustic;Enable Caustic;22;0;Create;True;0;0;0;False;2;Header(Caustic);Space;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;99;-719.0383,376.3457;Inherit;False;LocalCaustic;23;;604;3d52ce6591157a241a29e38f02bab76c;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-294.5063,67.39412;Inherit;False;Property;_Metallic;Metallic;8;0;Create;True;0;0;0;False;0;False;0.01;0.01;0.01;1;0;1;FLOAT;0
WireConnection;2;0;81;0
WireConnection;2;1;3;0
WireConnection;4;0;2;0
WireConnection;5;0;6;0
WireConnection;5;1;7;0
WireConnection;5;2;4;0
WireConnection;8;0;5;0
WireConnection;15;0;27;0
WireConnection;15;1;8;0
WireConnection;12;0;13;0
WireConnection;12;1;14;0
WireConnection;12;2;15;0
WireConnection;19;0;20;0
WireConnection;19;1;21;0
WireConnection;19;2;23;0
WireConnection;14;0;28;0
WireConnection;14;1;30;0
WireConnection;21;0;68;0
WireConnection;21;1;28;0
WireConnection;22;0;16;0
WireConnection;16;2;24;0
WireConnection;16;3;25;0
WireConnection;13;0;29;0
WireConnection;13;1;28;0
WireConnection;31;0;50;0
WireConnection;31;1;51;0
WireConnection;31;2;32;0
WireConnection;32;0;33;0
WireConnection;33;0;80;0
WireConnection;33;1;35;0
WireConnection;51;1;52;0
WireConnection;51;3;49;0
WireConnection;50;1;52;0
WireConnection;50;3;48;0
WireConnection;38;1;37;0
WireConnection;53;0;38;0
WireConnection;37;0;36;1
WireConnection;37;1;36;3
WireConnection;58;1;60;0
WireConnection;59;0;60;0
WireConnection;40;0;42;0
WireConnection;40;1;44;0
WireConnection;40;2;55;0
WireConnection;46;0;40;0
WireConnection;42;1;43;0
WireConnection;44;0;43;0
WireConnection;64;0;58;0
WireConnection;65;0;59;0
WireConnection;62;0;64;0
WireConnection;62;1;65;0
WireConnection;62;2;61;0
WireConnection;63;0;62;0
WireConnection;28;0;31;0
WireConnection;28;1;66;0
WireConnection;20;0;28;0
WireConnection;20;1;67;0
WireConnection;80;0;34;2
WireConnection;81;0;1;2
WireConnection;82;5;84;0
WireConnection;82;7;83;0
WireConnection;82;23;85;0
WireConnection;86;0;87;0
WireConnection;87;0;82;0
WireConnection;87;2;88;0
WireConnection;93;0;86;0
WireConnection;88;1;95;0
WireConnection;88;2;89;0
WireConnection;95;0;90;2
WireConnection;0;0;19;0
WireConnection;0;2;98;0
WireConnection;0;3;17;0
WireConnection;0;4;18;0
WireConnection;0;11;94;0
WireConnection;98;0;12;0
WireConnection;98;1;97;0
WireConnection;97;0;99;0
ASEEND*/
//CHKSM=BE29D03F8A627A1F3C32AD85BEC37B668C05DB4F