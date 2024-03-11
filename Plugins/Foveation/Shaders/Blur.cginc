#if !defined(BLUR)
#define BLUR

            float4 gausBlur5(sampler2D myTexture, float2 pos, float2 iResolution) // perform gaussian blur
            {
                //this will be our RGBA sum
                float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
                float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
                sum += tex2D(myTexture, pos + float2(-2,-2) * pixel_size) * 0.0165315806437010;
                sum += tex2D(myTexture, pos + float2(-2,-1) * pixel_size) * 0.0297018706890914;
                sum += tex2D(myTexture, pos + float2(-2, 0) * pixel_size) * 0.0361082918460354;
                sum += tex2D(myTexture, pos + float2(-2, 1) * pixel_size) * 0.0297018706890914;
                sum += tex2D(myTexture, pos + float2(-2, 2) * pixel_size) * 0.0165315806437010;

                sum += tex2D(myTexture, pos + float2(-1,-2) * pixel_size) * 0.0297018706890914;
                sum += tex2D(myTexture, pos + float2(-1,-1) * pixel_size) * 0.0533645960084072;
                sum += tex2D(myTexture, pos + float2(-1, 0) * pixel_size) * 0.0648748500418541;
                sum += tex2D(myTexture, pos + float2(-1, 1) * pixel_size) * 0.0533645960084072;
                sum += tex2D(myTexture, pos + float2(-1, 2) * pixel_size) * 0.0297018706890914;

                sum += tex2D(myTexture, pos + float2( 0,-2) * pixel_size) * 0.0361082918460354;
                sum += tex2D(myTexture, pos + float2( 0,-1) * pixel_size) * 0.0648748500418541;
                sum += tex2D(myTexture, pos + float2( 0, 0) * pixel_size) * 0.0788677603272776;
                sum += tex2D(myTexture, pos + float2( 0, 1) * pixel_size) * 0.0648748500418541;
                sum += tex2D(myTexture, pos + float2( 0, 2) * pixel_size) * 0.0361082918460354;

                sum += tex2D(myTexture, pos + float2( 1,-2) * pixel_size) * 0.0297018706890914;
                sum += tex2D(myTexture, pos + float2( 1,-1) * pixel_size) * 0.0533645960084072;
                sum += tex2D(myTexture, pos + float2( 1, 0) * pixel_size) * 0.0648748500418541;
                sum += tex2D(myTexture, pos + float2( 1, 1) * pixel_size) * 0.0533645960084072;
                sum += tex2D(myTexture, pos + float2( 1, 2) * pixel_size) * 0.0297018706890914;

                sum += tex2D(myTexture, pos + float2( 2,-2) * pixel_size) * 0.0165315806437010;
                sum += tex2D(myTexture, pos + float2( 2,-1) * pixel_size) * 0.0297018706890914;
                sum += tex2D(myTexture, pos + float2( 2, 0) * pixel_size) * 0.0361082918460354;
                sum += tex2D(myTexture, pos + float2( 2, 1) * pixel_size) * 0.0297018706890914;
                sum += tex2D(myTexture, pos + float2( 2, 2) * pixel_size) * 0.0165315806437010;
                return sum;
            }

            float4 gausBlur3(sampler2D myTexture, float2 pos, float2 iResolution) // perform gaussian blur
            {
                //this will be our RGBA sum
                float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
                float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
                sum += tex2D(myTexture, pos + float2(-1,-1) * pixel_size) * 0.0751;
                sum += tex2D(myTexture, pos + float2(-1, 0) * pixel_size) * 0.1238;
                sum += tex2D(myTexture, pos + float2(-1, 1) * pixel_size) * 0.0751;
                sum += tex2D(myTexture, pos + float2( 0,-1) * pixel_size) * 0.1238;
                sum += tex2D(myTexture, pos + float2( 0, 0) * pixel_size) * 0.2042;
                sum += tex2D(myTexture, pos + float2( 0, 1) * pixel_size) * 0.1238;
                sum += tex2D(myTexture, pos + float2( 1,-1) * pixel_size) * 0.0751;
                sum += tex2D(myTexture, pos + float2( 1, 0) * pixel_size) * 0.1238;
                sum += tex2D(myTexture, pos + float2( 1, 1) * pixel_size) * 0.0751;
                return sum;
            }

            float4 gausBlur11(sampler2D myTexture, float2 pos, float2 iResolution){
                float2 pixel_size = float2(1.0,1.0) / iResolution.xy;
                float4 sum = float4(0.0f,0.0f,0.0f,0.0f);
                sum += tex2D(myTexture, pos + float2(-5,-5) * pixel_size) * 0.007959;
                sum += tex2D(myTexture, pos + float2(-5,-4) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2(-5,-3) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2(-5,-2) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2(-5,-1) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2(-5, 0) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2(-5, 1) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2(-5, 2) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2(-5, 3) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2(-5, 4) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2(-5, 5) * pixel_size) * 0.007959;
                sum += tex2D(myTexture, pos + float2(-4,-5) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2(-4,-4) * pixel_size) * 0.008140;
                sum += tex2D(myTexture, pos + float2(-4,-3) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2(-4,-2) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2(-4,-1) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2(-4, 0) * pixel_size) * 0.008305;
                sum += tex2D(myTexture, pos + float2(-4, 1) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2(-4, 2) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2(-4, 3) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2(-4, 4) * pixel_size) * 0.008140;
                sum += tex2D(myTexture, pos + float2(-4, 5) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2(-3,-5) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2(-3,-4) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2(-3,-3) * pixel_size) * 0.008284;
                sum += tex2D(myTexture, pos + float2(-3,-2) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2(-3,-1) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2(-3, 0) * pixel_size) * 0.008378;
                sum += tex2D(myTexture, pos + float2(-3, 1) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2(-3, 2) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2(-3, 3) * pixel_size) * 0.008284;
                sum += tex2D(myTexture, pos + float2(-3, 4) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2(-3, 5) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2(-2,-5) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2(-2,-4) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2(-2,-3) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2(-2,-2) * pixel_size) * 0.008388;
                sum += tex2D(myTexture, pos + float2(-2,-1) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2(-2, 0) * pixel_size) * 0.008430;
                sum += tex2D(myTexture, pos + float2(-2, 1) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2(-2, 2) * pixel_size) * 0.008388;
                sum += tex2D(myTexture, pos + float2(-2, 3) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2(-2, 4) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2(-2, 5) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2(-1,-5) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2(-1,-4) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2(-1,-3) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2(-1,-2) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2(-1,-1) * pixel_size) * 0.008451;
                sum += tex2D(myTexture, pos + float2(-1, 0) * pixel_size) * 0.008462;
                sum += tex2D(myTexture, pos + float2(-1, 1) * pixel_size) * 0.008451;
                sum += tex2D(myTexture, pos + float2(-1, 2) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2(-1, 3) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2(-1, 4) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2(-1, 5) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2( 0,-5) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2( 0,-4) * pixel_size) * 0.008305;
                sum += tex2D(myTexture, pos + float2( 0,-3) * pixel_size) * 0.008378;
                sum += tex2D(myTexture, pos + float2( 0,-2) * pixel_size) * 0.008430;
                sum += tex2D(myTexture, pos + float2( 0,-1) * pixel_size) * 0.008462;
                sum += tex2D(myTexture, pos + float2( 0, 0) * pixel_size) * 0.008473;
                sum += tex2D(myTexture, pos + float2( 0, 1) * pixel_size) * 0.008462;
                sum += tex2D(myTexture, pos + float2( 0, 2) * pixel_size) * 0.008430;
                sum += tex2D(myTexture, pos + float2( 0, 3) * pixel_size) * 0.008378;
                sum += tex2D(myTexture, pos + float2( 0, 4) * pixel_size) * 0.008305;
                sum += tex2D(myTexture, pos + float2( 0, 5) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2( 1,-5) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2( 1,-4) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2( 1,-3) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2( 1,-2) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2( 1,-1) * pixel_size) * 0.008451;
                sum += tex2D(myTexture, pos + float2( 1, 0) * pixel_size) * 0.008462;
                sum += tex2D(myTexture, pos + float2( 1, 1) * pixel_size) * 0.008451;
                sum += tex2D(myTexture, pos + float2( 1, 2) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2( 1, 3) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2( 1, 4) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2( 1, 5) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2( 2,-5) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2( 2,-4) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2( 2,-3) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2( 2,-2) * pixel_size) * 0.008388;
                sum += tex2D(myTexture, pos + float2( 2,-1) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2( 2, 0) * pixel_size) * 0.008430;
                sum += tex2D(myTexture, pos + float2( 2, 1) * pixel_size) * 0.008420;
                sum += tex2D(myTexture, pos + float2( 2, 2) * pixel_size) * 0.008388;
                sum += tex2D(myTexture, pos + float2( 2, 3) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2( 2, 4) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2( 2, 5) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2( 3,-5) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2( 3,-4) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2( 3,-3) * pixel_size) * 0.008284;
                sum += tex2D(myTexture, pos + float2( 3,-2) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2( 3,-1) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2( 3, 0) * pixel_size) * 0.008378;
                sum += tex2D(myTexture, pos + float2( 3, 1) * pixel_size) * 0.008367;
                sum += tex2D(myTexture, pos + float2( 3, 2) * pixel_size) * 0.008336;
                sum += tex2D(myTexture, pos + float2( 3, 3) * pixel_size) * 0.008284;
                sum += tex2D(myTexture, pos + float2( 3, 4) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2( 3, 5) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2( 4,-5) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2( 4,-4) * pixel_size) * 0.008140;
                sum += tex2D(myTexture, pos + float2( 4,-3) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2( 4,-2) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2( 4,-1) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2( 4, 0) * pixel_size) * 0.008305;
                sum += tex2D(myTexture, pos + float2( 4, 1) * pixel_size) * 0.008295;
                sum += tex2D(myTexture, pos + float2( 4, 2) * pixel_size) * 0.008263;
                sum += tex2D(myTexture, pos + float2( 4, 3) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2( 4, 4) * pixel_size) * 0.008140;
                sum += tex2D(myTexture, pos + float2( 4, 5) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2( 5,-5) * pixel_size) * 0.007959;
                sum += tex2D(myTexture, pos + float2( 5,-4) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2( 5,-3) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2( 5,-2) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2( 5,-1) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2( 5, 0) * pixel_size) * 0.008212;
                sum += tex2D(myTexture, pos + float2( 5, 1) * pixel_size) * 0.008202;
                sum += tex2D(myTexture, pos + float2( 5, 2) * pixel_size) * 0.008171;
                sum += tex2D(myTexture, pos + float2( 5, 3) * pixel_size) * 0.008120;
                sum += tex2D(myTexture, pos + float2( 5, 4) * pixel_size) * 0.008049;
                sum += tex2D(myTexture, pos + float2( 5, 5) * pixel_size) * 0.007959;
                return sum;
            }
#endif