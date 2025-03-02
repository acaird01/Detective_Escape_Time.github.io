// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Davis3D/LowPolyOceanPack2/OceanLandspace"
{
	Properties
	{
		_BaseAlbedo("Base Albedo", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		_BaseSandNormal("Base Sand Normal", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Range( 0 , 8)) = 1
		_DetailSandNormal("Detail Sand Normal", 2D) = "bump" {}
		_DetailNormalIntensity("Detail Normal Intensity", Range( 0 , 8)) = 1
		_BaseTillingScale("Base Tilling Scale", Float) = 1
		_DetailTillingScale("Detail Tilling Scale", Float) = 1
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
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  "TerrainCompatible"="True" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _BaseSandNormal;
		uniform float _BaseTillingScale;
		uniform float _NormalIntensity;
		uniform sampler2D _DetailSandNormal;
		uniform float _DetailTillingScale;
		uniform float _DetailNormalIntensity;
		uniform sampler2D _BaseAlbedo;
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


		inline float4 TriplanarSampling1_g1( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
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


		inline float4 TriplanarSampling3_g1( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
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


		inline float4 TriplanarSampling6_g1( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_6_0 = ( _BaseTillingScale * i.uv_texcoord );
			o.Normal = BlendNormals( UnpackScaleNormal( tex2D( _BaseSandNormal, temp_output_6_0 ), _NormalIntensity ) , UnpackScaleNormal( tex2D( _DetailSandNormal, ( i.uv_texcoord * _DetailTillingScale ) ), _DetailNormalIntensity ) );
			o.Albedo = tex2D( _BaseAlbedo, temp_output_6_0 ).rgb;
			float2 temp_cast_1 = (( _Caustic1Scale * _CausticMasterScale )).xx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float temp_output_2_0_g4 = ( _Time.y * _Caustic1Speed );
			float4 triplanar1_g1 = TriplanarSampling1_g1( _CausticTexture, ( ase_worldPos + temp_output_2_0_g4 ), ase_worldNormal, 1.0, temp_cast_1, 1.0, 0 );
			float2 temp_cast_2 = (( _Caustic2Scale * _CausticMasterScale )).xx;
			float temp_output_2_0_g2 = ( _Time.y * _Caustic2Speed );
			float4 triplanar3_g1 = TriplanarSampling3_g1( _CausticTexture, ( ase_worldPos + temp_output_2_0_g2 ), ase_worldNormal, 1.0, temp_cast_2, 1.0, 0 );
			float2 temp_cast_3 = (( _Caustic3Scale * _CausticMasterScale )).xx;
			float temp_output_2_0_g3 = ( _Time.y * _Caustic3Speed );
			float4 triplanar6_g1 = TriplanarSampling6_g1( _CausticTexture, ( ase_worldPos + temp_output_2_0_g3 ), ase_worldNormal, 1.0, temp_cast_3, 1.0, 0 );
			#ifdef _ENABLECAUSTIC_ON
				float3 staticSwitch15 = ( (( ( ( ( triplanar1_g1.x + triplanar3_g1.x ) * _SubCausticIntensity ) * triplanar6_g1.x ) * _CausticColor )).rgb * _CausticIntensity );
			#else
				float3 staticSwitch15 = float3( 0,0,0 );
			#endif
			o.Emission = staticSwitch15;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
				float2 customPack1 : TEXCOORD1;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
Node;AmplifyShaderEditor.BlendNormalsNode;2;-305.0243,193.6233;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-797.6775,302.6815;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1194.242,-32.51499;Inherit;False;Property;_BaseTillingScale;Base Tilling Scale;7;0;Create;True;0;0;0;False;0;False;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1213.152,42.26757;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-972.6041,12.94315;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1012.885,321.8647;Inherit;False;Property;_DetailTillingScale;Detail Tilling Scale;8;0;Create;True;0;0;0;False;0;False;1;150;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-280.7028,18.43224;Inherit;False;Property;_Metallic;Metallic;1;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-276.7028,91.43224;Inherit;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-638.9707,83.10287;Inherit;True;Property;_BaseSandNormal;Base Sand Normal;3;0;Create;True;0;0;0;False;0;False;1;None;b0db36f71fa4d4a4888ae09188184638;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-637.2003,273.5934;Inherit;True;Property;_DetailSandNormal;Detail Sand Normal;5;0;Create;True;0;0;0;False;0;False;3;None;42f5b88ba0c544b42abfdfeb28642087;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-638.5866,-117.6913;Inherit;True;Property;_BaseAlbedo;Base Albedo;0;0;Create;True;0;0;0;False;0;False;4;abc00000000005938625728436909169;abc00000000005938625728436909169;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-962.5021,132.5148;Inherit;False;Property;_NormalIntensity;Normal Intensity;4;0;Create;True;0;0;0;False;0;False;1;0.4;0;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-946.3715,398.6117;Inherit;False;Property;_DetailNormalIntensity;Detail Normal Intensity;6;0;Create;True;0;0;0;False;0;False;1;0.75;0;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;101.1306,-46.02239;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Davis3D/LowPolyOceanPack2/OceanLandspace;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;1;TerrainCompatible=True;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.FunctionNode;14;-353.9553,340.8009;Inherit;False;LocalCaustic;10;;1;3d52ce6591157a241a29e38f02bab76c;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;15;-185.6608,311.9166;Inherit;False;Property;_EnableCaustic;Enable Caustic;9;0;Create;True;0;0;0;False;2;Header(Caustic);Space;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;8;0;5;0
WireConnection;8;1;9;0
WireConnection;6;0;7;0
WireConnection;6;1;5;0
WireConnection;1;1;6;0
WireConnection;1;5;12;0
WireConnection;3;1;8;0
WireConnection;3;5;13;0
WireConnection;4;1;6;0
WireConnection;0;0;4;0
WireConnection;0;1;2;0
WireConnection;0;2;15;0
WireConnection;0;3;10;0
WireConnection;0;4;11;0
WireConnection;15;0;14;0
ASEEND*/
//CHKSM=254B5D6FA7DD6C97F5775307A876297D8EA89660