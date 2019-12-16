// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GrassShader"
{
	Properties
	{
	 	_MainTex("Texture", 2D) = "white" {}
	    _HeightMap("HeightMap", 2D) = "white" {}
		_DirectionTexture("Direction Texture", 2D) = "white" {}
		_Wind("Wind Noise", 2D) = "white" {}

		_GrassLeafBottomColor("GrassLeafBottomColor", Color) = (0, 0.5, 0, 1)
		_GrassLeafTopColor("GrassLeafTopColor", Color) = (0, 1, 0, 1)
		[MaterialToogle]
	    _IsDense("IsDense", Float) = 0
		[MaterialToogle]
		_AddOriginalGeometry("AddOriginalGeometry", Float) = 1
		_MaxHeight("MaxHeight", Float) = 1
		_MaxWidth("MaxWidth", Float) = 0.1
		_UnitHeightSegmentBottom("UnitHeightSegmentBottom", Float) = 0.2
		_UnitHeightSegmentMid("UnitHeightSegmentMid", Float) = 0.3
		_UnitHeightSegmentTop("UnitHeightSegmentTop", Float) = 0.4
		_UnitWidthSegmentBottom("UnitWidthSegmentBottom", Float) = 0.3
		_UnitWidthSegmentMid("UnitWidthSegmentMid", Float) = 0.2
		_UnitWidthSegmentTop("UnitWidthSegmentTop", Float) = 0.1
		_BendSegmentBottom("BendSegmentBottom", Float) = 0
		_BendSegmentMid("BendSegmentMid", Float) = 0
		_BendSegmentTop("BendSegmentTop", Float) = 0

		_WindFrequency("Wind Frecuency", Float) = 1
		_WindMin("WindMin", Float) = 0
		_WindMax("WindMax", Float) = 1
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma geometry geom
				#pragma fragment frag

			// Base properties
			bool _IsDense;
			bool _AddOriginalGeometry;
			float _MaxHeight;
			float _MaxWidth;
			float _UnitHeightSegmentBottom;
			float _UnitHeightSegmentMid;
			float _UnitHeightSegmentTop;
			float _UnitWidthSegmentBottom;
			float _UnitWidthSegmentMid;
			float _UnitWidthSegmentTop;
			float _BendSegmentBottom;
			float _BendSegmentMid;
			float _BendSegmentTop;
			float4 _GrassLeafBottomColor;
			float4 _GrassLeafTopColor;
			float _WindFrequency;
			float _WindMin;
			float _WindMax;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _HeightMap;
			float4 _HeightMap_ST;
			sampler2D _DirectionTexture;
			float4 _DirectionTexture_ST;
			sampler2D _Wind;
			float4 _Wind_ST;

			struct vertexInput
			{
				float4 Position : POSITION;
				float3 Normal : NORMAL;
				float2 TexCoord : TEXCOORD0;
			};

			// This struct is the input data from the geometry shader.
			// Simply convert the data from the vertex shader to world position
			struct geomInput
			{
				float4 Position : SV_POSITION;
				float3 Normal : NORMAL;
				float2 TexCoord : TEXCOORD0;
			};

			struct fragInput {
				float4 Position : SV_POSITION;
				float3 Normal : NORMAL;
				float2 TexCoord : TEXCOORD0;
				float4 grassLeafColor : COLOR;
			};

			geomInput vert(vertexInput v)
			{
				//to world space transform
				geomInput g;
				g.Position = mul(unity_ObjectToWorld, float4(v.Position));
				g.Normal = mul(v.Normal, unity_WorldToObject);
				g.TexCoord = TRANSFORM_TEX(v.TexCoord, _MainTex);
				return g;
			}

			// Remap given value s from the first range [a1,a2] to the second range [b1,b2]
			float remap(float s, float a1, float a2, float b1, float b2)
			{
				return b1 + (s - a1)*(b2 - b1) / (a2 - a1);
			}

			void CreateVertex(inout TriangleStream<fragInput> triStream, float4 pos, float3 nor, 
				float2 texCoord, float4 color) {
				fragInput fi;
				fi.Position = mul(UNITY_MATRIX_VP, pos);
				fi.Normal = nor;
				fi.TexCoord = texCoord;
				fi.grassLeafColor = color;
				triStream.Append(fi);
			}

			void CreateGrass(inout TriangleStream<fragInput> triStream, float height, float direction,
				float4 P1, float4 P2, float4 P3, float3 N1, float3 N2, float3 N3)
			{
				//1: Calculate basepoints to start at 
				float4 basePoint = (P1 + P2 + P3) / 3;
				float4 normalbasepoint = float4((N1 + N2 + N3) / 3, 0);

				//2: Calculate segment height, width and total height, width
				float grassHeight = height * _MaxHeight; 
				float segmentBottomHeight = grassHeight * _UnitHeightSegmentBottom;
				float segmentMidHeight = grassHeight * _UnitHeightSegmentMid;
				float segmentTopHeight = grassHeight * _UnitHeightSegmentTop;

				float grassWidth = _MaxWidth;
			    float segmentBottomWidth = grassWidth * _UnitWidthSegmentBottom;
				float segmentMidWidth = grassWidth * _UnitWidthSegmentMid;
				float segmentTopWidth = grassWidth * _UnitWidthSegmentTop;

				//3: initial direction in which to generate the grass blades
				direction -= -0.5; //make direction range from [0,1] to [-0.5, 0.5]
				float4 grassDirection = normalize((P2 - P1) * direction);

				//Animation
				//Sample wind noise and normalize, we want only a direction
				float3 wind = normalize(tex2Dlod(_Wind, float4(basePoint.xz * _Wind_ST.xy + _Wind_ST.zw * _Time.w, 0, 0)).xyz);
				// And here it is : we convert our wind to a vector in the [-1,1] range.
				// As for the sinus, we use the time value modulated by the frequency.
				// And finally, we remap it between our minimum and maximum values and use it to scale our wind normal.
				float3 windNormal = (wind * 2 - 1) * remap(sin(_Time.w*_WindFrequency), -1, 1, _WindMin, _WindMax);
				// Of course, we need to add it to our provided normal.
				normalbasepoint = normalize(float4((normalbasepoint + windNormal).xyz, 0));

				//4: calculate the positions for each vertex
				float4 v[7]; //trianglestrip 
				v[0] = basePoint - grassDirection * segmentBottomWidth;
				v[1] = basePoint + grassDirection * segmentBottomWidth;
				v[2] = basePoint - (grassDirection * segmentMidWidth) + (segmentBottomHeight * normalbasepoint);
				v[3] = basePoint + (grassDirection * segmentMidWidth) + (segmentBottomHeight * normalbasepoint);

				v[4] = v[3] - ((grassDirection) * segmentTopWidth) + (segmentMidHeight * normalbasepoint);
				v[5] = v[3] + ((grassDirection) * segmentTopWidth) + (segmentMidHeight * normalbasepoint);
				v[6] = v[5] + ((grassDirection) * segmentTopWidth) + (segmentTopHeight * normalbasepoint);

				grassDirection = float4(1, 0, 0, 0);

				v[2] += (grassDirection * _BendSegmentBottom);
				v[3] += (grassDirection * _BendSegmentBottom);
				v[4] += (grassDirection * _BendSegmentMid);
				v[5] += (grassDirection * _BendSegmentMid);
				v[6] += (grassDirection * _BendSegmentTop);

				//6: create the vertices with a helper method 
				CreateVertex(triStream, v[0], float3(0,0,0), float2(0,0), _GrassLeafBottomColor); 
				CreateVertex(triStream, v[1], float3(0,0,0), float2(0.5,0), _GrassLeafBottomColor);
				CreateVertex(triStream, v[2], float3(0,0,0), float2(0.3,0.3), _GrassLeafBottomColor);
				CreateVertex(triStream, v[3], float3(0,0,0), float2(0.6,0.3), _GrassLeafBottomColor);
				CreateVertex(triStream, v[4], float3(0,0,0), float2(0.6,0.3), _GrassLeafTopColor);
				CreateVertex(triStream, v[5], float3(0,0,0), float2(0.9,0.6), _GrassLeafTopColor);
				CreateVertex(triStream, v[6], float3(0,0,0), float2(1,1), _GrassLeafTopColor);
				//now we create the triangles for the back face going backwards
				CreateVertex(triStream, v[5], float3(0, 0, 0), float2(0.9, 0.6), _GrassLeafTopColor);
				CreateVertex(triStream, v[4], float3(0, 0, 0), float2(0.6, 0.3), _GrassLeafTopColor);
				CreateVertex(triStream, v[3], float3(0, 0, 0), float2(0.6, 0.3), _GrassLeafBottomColor);
				CreateVertex(triStream, v[2], float3(0, 0, 0), float2(0.3, 0.3), _GrassLeafBottomColor);
				CreateVertex(triStream, v[1], float3(0, 0, 0), float2(0.5, 0), _GrassLeafBottomColor);
				CreateVertex(triStream, v[0], float3(0, 0, 0), float2(0, 0), _GrassLeafBottomColor);

				triStream.RestartStrip();
			}

			[maxvertexcount(70)]
			void geom(triangle geomInput IN[3], inout TriangleStream<fragInput> triStream)
			{
				// Because access the input data directly tend to make the code a mess, I usually repack everything in clean variables
				float4 P1 = IN[0].Position;
				float4 P2 = IN[1].Position;
				float4 P3 = IN[2].Position;

				float4 N1 = float4(IN[0].Normal, 0);
				float4 N2 = float4(IN[1].Normal, 0);
				float4 N3 = float4(IN[2].Normal, 0);

				//add original geometry
				if (_AddOriginalGeometry == 1) { 
					CreateVertex(triStream, P1, N1, float2(0, 0), _GrassLeafBottomColor); 
					CreateVertex(triStream, P2, N2, float2(0, 0), _GrassLeafBottomColor);
					CreateVertex(triStream, P3, N3, float2(0, 0), _GrassLeafBottomColor);
					triStream.RestartStrip();
				} 
				//sample height and direction noise maps 
				float samplePoint = tex2Dlod(_HeightMap, float4(IN[0].TexCoord.xy, 0, 0)).r;
				float samplePoint2 = tex2Dlod(_HeightMap, float4(IN[1].TexCoord.xy, 0, 0)).r;
				float samplePoint3 = tex2Dlod(_HeightMap, float4(IN[2].TexCoord.xy, 0, 0)).r;

				float directionSamplePoint = tex2Dlod(_DirectionTexture, float4(IN[0].TexCoord.xy, 0, 0)).r;
				float directionSamplePoint2 = tex2Dlod(_DirectionTexture, float4(IN[1].TexCoord.xy, 0, 0)).r;
				float directionSamplePoint3 = tex2Dlod(_DirectionTexture, float4(IN[2].TexCoord.xy, 0, 0)).r;

				//split the received triangle in 3 sub-triangles
				if (_IsDense == 1) { 
					float4 m0 = (P1 + P2) * 0.5;
					float4 m1 = (P2 + P3) * 0.5; 
					float4 m2 = (P3 + P1) * 0.5; 
					CreateGrass(triStream, samplePoint, directionSamplePoint, m1, P2, m0, N1, N2, N3); 
					CreateGrass(triStream, samplePoint2, directionSamplePoint2, P1, m0, m2, N1, N2, N3); 
					CreateGrass(triStream, samplePoint3, directionSamplePoint3, m2, m1, P3, N1, N2, N3);
				}
				else { 
					CreateGrass(triStream, samplePoint, directionSamplePoint, P1, P2, P3, N1, N2, N3);
				}
			}

			//The interpolation made by GPU do the trick with the color
			fixed4 frag(fragInput f) : SV_Target
			{
				//return f.grassLeafColor;
				return tex2D(_MainTex, f.TexCoord) * f.grassLeafColor;
				//return tex2D(_MainTex, f.TexCoord);
			}

			ENDCG
		}
	}
}
