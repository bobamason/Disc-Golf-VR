// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/Vertex-Offset" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_maxTunnelDistance("Max Tunnel Distance", Float) = 100.0
		_offset("offset", Vector) = (0.0, 0.0, 0.0)
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd vertex:disp

		struct appdata {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
			};

			float3 _offset;
			float _maxTunnelDistance;

			void disp(inout appdata v)
			{

				float4 pos = mul(unity_ObjectToWorld, v.vertex);

				// strength should be a float between 0.0 and 1.0, I just divided by the length of the tunnel
				float strength = length(pos.xyz - _WorldSpaceCameraPos) / _maxTunnelDistance;

				// square the strength variable since cubic interpolation results in a better looking curve
				v.vertex.xyz += _offset * (strength * strength);
			}

	sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}

		Fallback "Mobile/Diffuse"
}
