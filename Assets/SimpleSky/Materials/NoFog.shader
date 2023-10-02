Shader "Unlit/NoFog" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Fog { Mode Off }
        Lighting Off
    
        Pass {
            SetTexture [_MainTex] { combine texture }
        }
    }
}