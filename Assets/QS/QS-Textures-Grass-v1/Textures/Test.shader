Shader "Custom/RandomTilingMask" {
    Properties {
        _Tex1 ("Texture", 2D) = "white" {}
        [Toggle] _UseRandMask ("Use Random Mask", Int) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
   
        CGPROGRAM
        #pragma surface surf Lambert
        sampler2D _Tex1;
        float _UseRandMask;
     
        struct Input {
            float2 uv_Tex1;
            float3 worldPos;
        };
        // generic pseudo-random function
        float rand2 ( float2 coords ){
            return frac(sin(dot(coords, float2(12.9898,78.233))) * 43758.5453);
        }
        void surf (Input IN, inout SurfaceOutput o) {
                           
            if ( _UseRandMask == 1 ) {
                // calculate rotation matrix parameters from the original UV data
                half2 samp = IN.uv_Tex1;
                half r = (round(rand2(floor(samp))*3));
                half m1 = ((r-1)*(3-r))/min(r-3, -1);
                half m2 = (r*(2-r))/max(r,1);
                half m3 = (r*(r-2))/max(r,1);
                half m4 = ((3-r)*(r-1))/min(r-3, -1);
               
                // rotate texture UVs based on the calculated rotation matrix parameters
                samp -= 0.5;
                samp = mul(samp, float2x2(m1, m2, m3, m4));
                samp.xy += 0.5;
               
                // use input texture with calculated UVs
                half4 tex1 = tex2D (_Tex1, samp);
                o.Albedo = tex1.rgb;
           
            } else {
                // _UseRandMask == 0
                // show the original texture without UV rotation
                half4 tex1 = tex2D (_Tex1, IN.uv_Tex1);
                o.Albedo = tex1.rgb;
            }
       
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}