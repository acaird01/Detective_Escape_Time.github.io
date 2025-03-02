// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Davis3D/LowPolyOceanPack2/OceanSurface"
{
	Properties
	{
		_Cubemap("Cubemap", 2D) = "white" {}
		_Cubemap_Brightness("Cubemap_Brightness", Float) = 1
		_Cubemap_Power("Cubemap_Power", Float) = 1
		_NormalFresnelScale("Normal Fresnel Scale", Float) = 5
		_NormalFresnelPower("Normal Fresnel Power", Float) = 1
		_NormalIntensity("Normal Intensity", Float) = 1
		_Normal("Normal", 2D) = "bump" {}
		_Water_Tile1("Water_Tile1", Float) = 50
		_Water_Tile2("Water_Tile2", Float) = 40
		_Water_Tile3("Water_Tile3", Float) = 50
		_Water_Tile4("Water_Tile4", Float) = 40
		_Water_MASTER_Scale("Water_MASTER_Scale", Float) = 1
		_Water_Speed1("Water_Speed1", Float) = 0.1
		_Water_Speed2("Water_Speed2", Float) = 0.1
		_Water_Speed3("Water_Speed3", Float) = 0.1
		_Water_Speed4("Water_Speed4", Float) = 0.1
		_SPEED_MASTER("SPEED_MASTER", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		uniform sampler2D _Cubemap;
		uniform sampler2D _Normal;
		uniform float _SPEED_MASTER;
		uniform float _Water_Speed1;
		uniform float _Water_Tile1;
		uniform float _Water_MASTER_Scale;
		uniform float _Water_Speed2;
		uniform float _Water_Tile2;
		uniform float _Water_Speed3;
		uniform float _Water_Tile3;
		uniform float _Water_Speed4;
		uniform float _Water_Tile4;
		uniform float _NormalIntensity;
		uniform float _NormalFresnelScale;
		uniform float _NormalFresnelPower;
		uniform float _Cubemap_Power;
		uniform float _Cubemap_Brightness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float3 break28 = reflect( ase_worldViewDir , ase_normWorldNormal );
			float3 appendResult29 = (float3(break28.x , break28.y , ( break28.z * -1.0 )));
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 worldToTangentDir88 = mul( ase_worldToTangent, appendResult29);
			float3 _NormalTN = float3(0,0,1);
			float2 temp_cast_0 = (( _SPEED_MASTER * _Water_Speed1 )).xx;
			float2 temp_output_59_0 = ( i.uv_texcoord * _Water_MASTER_Scale );
			float2 panner53 = ( 1.0 * _Time.y * temp_cast_0 + ( _Water_Tile1 * temp_output_59_0 ));
			float2 temp_cast_1 = (( _SPEED_MASTER * _Water_Speed2 )).xx;
			float2 panner54 = ( 1.0 * _Time.y * temp_cast_1 + ( _Water_Tile2 * temp_output_59_0 ));
			float2 temp_cast_2 = (( _SPEED_MASTER * _Water_Speed3 )).xx;
			float2 panner55 = ( 1.0 * _Time.y * temp_cast_2 + ( _Water_Tile3 * temp_output_59_0 ));
			float2 temp_cast_3 = (( _SPEED_MASTER * _Water_Speed4 )).xx;
			float2 panner56 = ( 1.0 * _Time.y * temp_cast_3 + ( _Water_Tile4 * temp_output_59_0 ));
			float3 lerpResult33 = lerp( _NormalTN , BlendNormals( BlendNormals( UnpackNormal( tex2D( _Normal, panner53 ) ) , UnpackNormal( tex2D( _Normal, panner54 ) ) ) , BlendNormals( UnpackNormal( tex2D( _Normal, panner55 ) ) , UnpackNormal( tex2D( _Normal, panner56 ) ) ) ) , _NormalIntensity);
			float fresnelNdotV42 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode42 = ( 0.0 + _NormalFresnelScale * pow( max( 1.0 - fresnelNdotV42 , 0.0001 ), _NormalFresnelPower ) );
			float3 lerpResult32 = lerp( _NormalTN , lerpResult33 , fresnelNode42);
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float3 tangentToWorldDir91 = mul( ase_tangentToWorldFast, BlendNormals( worldToTangentDir88 , lerpResult32 ) );
			float4 temp_cast_5 = (_Cubemap_Power).xxxx;
			float4 clampResult3 = clamp( tex2D( _Cubemap, tangentToWorldDir91.xy ) , float4( -100,0,0,0 ) , temp_cast_5 );
			o.Emission = ( clampResult3 * _Cubemap_Brightness ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
Node;AmplifyShaderEditor.RangedFloatNode;60;-4143.361,121.709;Inherit;False;Property;_Water_MASTER_Scale;Water_MASTER_Scale;11;0;Create;True;0;0;0;False;0;False;1;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-4131.636,-7.029883;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;79;-3503.316,377.015;Inherit;False;Property;_Water_Speed4;Water_Speed4;15;0;Create;True;0;0;0;False;0;False;0.1;-0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-3524.026,54.46709;Inherit;False;Property;_Water_Tile3;Water_Tile3;9;0;Create;True;0;0;0;False;0;False;50;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-4120.764,-114.7251;Inherit;False;Property;_SPEED_MASTER;SPEED_MASTER;16;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-3882.599,55.31087;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-3530.581,153.3974;Inherit;False;Property;_Water_Speed3;Water_Speed3;14;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-3486.619,255.2129;Inherit;False;Property;_Water_Tile4;Water_Tile4;10;0;Create;True;0;0;0;False;0;False;40;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-3313.082,155.7024;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-3289.275,359.7245;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-3538.974,-25.09189;Inherit;False;Property;_Water_Speed2;Water_Speed2;13;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-3285.194,266.4154;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-3519.42,-201.1778;Inherit;False;Property;_Water_Speed1;Water_Speed1;12;0;Create;True;0;0;0;False;0;False;0.1;-0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-3313.501,61.76958;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-3485.819,-310.1911;Inherit;False;Property;_Water_Tile1;Water_Tile1;7;0;Create;True;0;0;0;False;0;False;50;150;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-3508.864,-117.9577;Inherit;False;Property;_Water_Tile2;Water_Tile2;8;0;Create;True;0;0;0;False;0;False;40;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-3313.768,-209.9478;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;56;-3126.47,256.4999;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;55;-3133.97,67.79986;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-3314.564,-112.8027;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-3314.987,-25.69186;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-3314.083,-297.9111;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;51;-2962.76,230.4113;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Instance;48;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;54;-3145.443,-114.4166;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;50;-2962.634,45.70163;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Instance;48;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;53;-3140.001,-294.2644;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;49;-2962.385,-136.5703;Inherit;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Instance;48;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;48;-2960.231,-322.9493;Inherit;True;Property;_Normal;Normal;6;0;Create;True;0;0;0;False;0;False;-1;abc00000000017054413239576147454;abc00000000017054413239576147454;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;46;-2614.546,-237.2487;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;36;-2355.075,-53.47897;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;47;-2625.656,139.8831;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;32;-1832.796,-196.3197;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;33;-2121.221,-76.46904;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;37;-2318.799,-200.2063;Inherit;False;Constant;_NormalTN;Normal TN;6;0;Create;True;0;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;29;-2033.119,-494.6581;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;28;-2274.485,-489.5902;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TransformDirectionNode;88;-1886.479,-499.6085;Inherit;False;World;Tangent;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BlendNormalsNode;21;-1617.231,-329.4257;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;17;-1177.689,-354.9973;Inherit;True;Property;_Cubemap;Cubemap;0;0;Create;True;0;0;0;False;0;False;-1;699b2fd6b48f7374bad37c7b1dd09a87;699b2fd6b48f7374bad37c7b1dd09a87;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformDirectionNode;91;-1397.319,-332.3428;Inherit;False;Tangent;World;False;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;41;-2390.542,406.6577;Inherit;False;Property;_NormalFresnelPower;Normal Fresnel Power;4;0;Create;True;0;0;0;False;0;False;1;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-2384.542,336.6578;Inherit;False;Property;_NormalFresnelScale;Normal Fresnel Scale;3;0;Create;True;0;0;0;False;0;False;5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;3;-859.7722,-347.938;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;-100,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1075.01,-159.2426;Inherit;False;Property;_Cubemap_Power;Cubemap_Power;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-862.3098,-237.941;Inherit;False;Property;_Cubemap_Brightness;Cubemap_Brightness;1;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-621.9886,-348.127;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-462.4481,-396.9764;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Davis3D/LowPolyOceanPack2/OceanSurface;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.FresnelNode;42;-2118.542,268.6577;Inherit;False;Standard;WorldNormal;ViewDir;False;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;25;-2633.686,-561.4095;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;-2163.8,-417.7484;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;27;-2652.441,-418.5496;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ReflectOpNode;23;-2448.421,-489.9264;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-2355.382,60.08543;Inherit;False;Property;_NormalIntensity;Normal Intensity;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
WireConnection;59;0;58;0
WireConnection;59;1;60;0
WireConnection;77;0;72;0
WireConnection;77;1;78;0
WireConnection;80;0;72;0
WireConnection;80;1;79;0
WireConnection;68;0;67;0
WireConnection;68;1;59;0
WireConnection;66;0;65;0
WireConnection;66;1;59;0
WireConnection;73;0;72;0
WireConnection;73;1;75;0
WireConnection;56;0;68;0
WireConnection;56;2;80;0
WireConnection;55;0;66;0
WireConnection;55;2;77;0
WireConnection;64;0;63;0
WireConnection;64;1;59;0
WireConnection;74;0;72;0
WireConnection;74;1;76;0
WireConnection;61;0;62;0
WireConnection;61;1;59;0
WireConnection;51;1;56;0
WireConnection;54;0;64;0
WireConnection;54;2;74;0
WireConnection;50;1;55;0
WireConnection;53;0;61;0
WireConnection;53;2;73;0
WireConnection;49;1;54;0
WireConnection;48;1;53;0
WireConnection;46;0;48;0
WireConnection;46;1;49;0
WireConnection;36;0;46;0
WireConnection;36;1;47;0
WireConnection;47;0;50;0
WireConnection;47;1;51;0
WireConnection;32;0;37;0
WireConnection;32;1;33;0
WireConnection;32;2;42;0
WireConnection;33;0;37;0
WireConnection;33;1;36;0
WireConnection;33;2;40;0
WireConnection;29;0;28;0
WireConnection;29;1;28;1
WireConnection;29;2;89;0
WireConnection;28;0;23;0
WireConnection;88;0;29;0
WireConnection;21;0;88;0
WireConnection;21;1;32;0
WireConnection;17;1;91;0
WireConnection;91;0;21;0
WireConnection;3;0;17;0
WireConnection;3;2;15;0
WireConnection;2;0;3;0
WireConnection;2;1;4;0
WireConnection;0;2;2;0
WireConnection;42;2;43;0
WireConnection;42;3;41;0
WireConnection;89;0;28;2
WireConnection;23;0;25;0
WireConnection;23;1;27;0
ASEEND*/
//CHKSM=1ADDBC6F0E153B80B7FB58D379D4ED68067088F7