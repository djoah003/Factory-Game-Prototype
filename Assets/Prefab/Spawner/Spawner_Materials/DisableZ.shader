Shader "Custom/DisableZ"
{
	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}

		Pass{
			ZWrite Off
		}
	}
}
