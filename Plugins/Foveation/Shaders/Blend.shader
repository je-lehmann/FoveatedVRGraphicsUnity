Shader "Custom/Blend"
{
    Properties
    {
        _FovealTex ("Texture", 2D) = "red" {}
        _PeriphericalTex ("Texture", 2D) = "red" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Blur.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _FovealTex;
            sampler2D _PeriphericalTex;
            uniform float _eyeX;
            uniform float _eyeY;
            uniform float _fovealReduction;
          /*  uniform float _reductionFactor;
            uniform float _contrastParam;
            uniform int _applyPeriphericalContrast;
            uniform int _blurActive; */

            //linear blend from 100 to 0 %
            fixed2 calcFallOff(float minDis, float maxDis) {
                fixed2 result;
                result.x = -1.0 / (maxDis - minDis);
                result.y = -result.x * maxDis;
                return result;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                _eyeX = _eyeX * _fovealReduction;
                _eyeY = _eyeY * _fovealReduction;

                //foveal radii
                float r1 = 0.35;
                float r2 = 0.45;

                //render layers around focus
                float dist = length(i.uv * _fovealReduction - float2(_eyeX, _eyeY));
                float2 fovealKoords =  float2(( i.uv * _fovealReduction ) - float2(_eyeX, _eyeY) + float2(0.5, 0.5)); //calculation of scaled and eye related koords
                fixed4 pcol = tex2D(_PeriphericalTex, i.uv); 
                fixed4 fovcol = tex2D(_FovealTex, fovealKoords);

                //map layers according to distance from each focuspoint:
                //Foveal
                if(dist < r1){ 
                    col = fovcol;
                }
                //Blending between peripherical and foveal texture 
                else if (dist >= r1 && dist < r2){
                    fixed2 param = calcFallOff(r1, r2);
                    col = lerp(pcol, fovcol, param.x * dist + param.y);
                }
                //peripherical
                else{
                    col = pcol;
                }  

                //Can be used to visualize foveal Radius
                /*if(dist > r1 && dist < r1 + 0.01){
                    col = float4(1,0,0,0);
                }

                if(dist > r2 && dist < r2 + 0.01){
                    col = float4(0,0,1,0);
                } */
                return col;
            }
            ENDCG
        }
    }
}
