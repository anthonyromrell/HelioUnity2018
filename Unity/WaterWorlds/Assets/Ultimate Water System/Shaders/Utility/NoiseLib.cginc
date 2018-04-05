//
//	It's just a port for Unity of cool NoiseLib by Brian Sharpe originally written in glsl.
//	Original copyright notice:
// 
//	Code repository for GPU noise development blog 
//	http://briansharpe.wordpress.com 
//	https://github.com/BrianSharpe 
// 
//	I'm not one for copyrights.  Use the code however you wish. 
//	All I ask is that credit be given back to the blog or myself when appropriate. 
//	And also to let me know if you come up with any changes, improvements, thoughts or interesting uses for this stuff. :) 
//	Thanks! 
// 
//	Brian Sharpe 
//	brisharpe CIRCLE_A yahoo DOT com 
//	http://briansharpe.wordpress.com 
//	https://github.com/BrianSharpe 
// 



// 
//	Interpolation functions 
//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 ) 
//	http://briansharpe.wordpress.com/2011/11/14/two-useful-interpolation-functions-for-noise-development/ 
// 
float Interpolation_C1(float x) { return x * x * (3.0 - 2.0 * x); }   //  3x^2-2x^3  ( Hermine Curve.  Same as SmoothStep().  As used by Perlin in Original Noise. ) 
float2 Interpolation_C1(float2 x) { return x * x * (3.0 - 2.0 * x); }
float3 Interpolation_C1(float3 x) { return x * x * (3.0 - 2.0 * x); }
float4 Interpolation_C1(float4 x) { return x * x * (3.0 - 2.0 * x); }


float Interpolation_C2(float x) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }   //  6x^5-15x^4+10x^3	( Quintic Curve.  As used by Perlin in Improved Noise.  http://mrl.nyu.edu/~perlin/paper445.pdf ) 
float2 Interpolation_C2(float2 x) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }
float3 Interpolation_C2(float3 x) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }
float4 Interpolation_C2(float4 x) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }



float4 FAST32_hash_2D(float2 gridcell)	//	generates a random number for each of the 4 cell corners
{
    //	gridcell is assumed to be an integer coordinate
    const float2 OFFSET = float2(26.0, 161.0);
    const float DOMAIN = 71.0;
    const float SOMELARGEFLOAT = 951.135664;
    float4 P = float4(gridcell.xy, gridcell.xy + 1.0);
    P = P - floor(P * (1.0 / DOMAIN)) * DOMAIN;	//	truncate the domain
    P += OFFSET.xyxy;								//	offset to interesting part of the noise
    P *= P;											//	calculate and return the hash
    return frac(P.xzxz * P.yyww * (1.0 / SOMELARGEFLOAT));
}
void FAST32_hash_2D(float2 gridcell, out float4 hash_0, out float4 hash_1)	//	generates 2 random numbers for each of the 4 cell corners
{
    //    gridcell is assumed to be an integer coordinate
    const float2 OFFSET = float2(26.0, 161.0);
    const float DOMAIN = 71.0;
    const float2 SOMELARGEFLOATS = float2(951.135664, 642.949883);
    float4 P = float4(gridcell.xy, gridcell.xy + 1.0);
    P = P - floor(P * (1.0 / DOMAIN)) * DOMAIN;
    P += OFFSET.xyxy;
    P *= P;
    P = P.xzxz * P.yyww;
    hash_0 = frac(P * (1.0 / SOMELARGEFLOATS.x));
    hash_1 = frac(P * (1.0 / SOMELARGEFLOATS.y));
}
void FAST32_hash_2D(float2 gridcell,
    out float4 hash_0,
    out float4 hash_1,
    out float4 hash_2)	//	generates 3 random numbers for each of the 4 cell corners
{
    //    gridcell is assumed to be an integer coordinate
    const float2 OFFSET = float2(26.0, 161.0);
    const float DOMAIN = 71.0;
    const float3 SOMELARGEFLOATS = float3(951.135664, 642.949883, 803.202459);
    float4 P = float4(gridcell.xy, gridcell.xy + 1.0);
    P = P - floor(P * (1.0 / DOMAIN)) * DOMAIN;
    P += OFFSET.xyxy;
    P *= P;
    P = P.xzxz * P.yyww;
    hash_0 = frac(P * (1.0 / SOMELARGEFLOATS.x));
    hash_1 = frac(P * (1.0 / SOMELARGEFLOATS.y));
    hash_2 = frac(P * (1.0 / SOMELARGEFLOATS.z));
}

void FAST32_hash_3D(float3 gridcell,
    out float4 lowz_hash_0,
    out float4 lowz_hash_1,
    out float4 lowz_hash_2,
    out float4 highz_hash_0,
    out float4 highz_hash_1,
    out float4 highz_hash_2)		//	generates 3 random numbers for each of the 8 cell corners 
{
    //    gridcell is assumed to be an integer coordinate 


    //	TODO: 	these constants need tweaked to find the best possible noise. 
    //			probably requires some kind of brute force computational searching or something.... 
    const float2 OFFSET = float2(50.0, 161.0);
    const float DOMAIN = 69.0;
    const float3 SOMELARGEFLOATS = float3(635.298681, 682.357502, 668.926525);
    const float3 ZINC = float3(48.500388, 65.294118, 63.934599);
    const float DOMAINMINUS = DOMAIN - 1.5;


    //	truncate the domain 
    gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * (1.0 / DOMAIN)) * DOMAIN;
    float3 gridcell_inc1 = step(gridcell, float3(DOMAINMINUS, DOMAINMINUS, DOMAINMINUS)) * (gridcell + 1.0);


    //	calculate the noise 
    float4 P = float4(gridcell.xy, gridcell_inc1.xy) + OFFSET.xyxy;
    P *= P;
    P = P.xzxz * P.yyww;
    float3 lowz_mod = float3(1.0 / (SOMELARGEFLOATS.xyz + gridcell.zzz * ZINC.xyz));
    float3 highz_mod = float3(1.0 / (SOMELARGEFLOATS.xyz + gridcell_inc1.zzz * ZINC.xyz));
    lowz_hash_0 = frac(P * lowz_mod.xxxx);
    highz_hash_0 = frac(P * highz_mod.xxxx);
    lowz_hash_1 = frac(P * lowz_mod.yyyy);
    highz_hash_1 = frac(P * highz_mod.yyyy);
    lowz_hash_2 = frac(P * lowz_mod.zzzz);
    highz_hash_2 = frac(P * highz_mod.zzzz);
}


float Perlin3D(float3 P)
{
    //	establish our grid cell and unit position 
    float3 Pi = floor(P);
    float3 Pf = P - Pi;
    float3 Pf_min1 = Pf - 1.0;


    #if 1 
    // 
    //	classic noise. 
    //	requires 3 random values per point.  with an efficent hash function will run faster than improved noise 
    // 


    //	calculate the hash. 
    //	( various hashing methods listed in order of speed ) 
    float4 hashx0, hashy0, hashz0, hashx1, hashy1, hashz1;
    FAST32_hash_3D(Pi, hashx0, hashy0, hashz0, hashx1, hashy1, hashz1);
    //SGPP_hash_3D( Pi, hashx0, hashy0, hashz0, hashx1, hashy1, hashz1 ); 


    //	calculate the gradients 
    float4 grad_x0 = hashx0 - 0.49999;
    float4 grad_y0 = hashy0 - 0.49999;
    float4 grad_z0 = hashz0 - 0.49999;
    float4 grad_x1 = hashx1 - 0.49999;
    float4 grad_y1 = hashy1 - 0.49999;
    float4 grad_z1 = hashz1 - 0.49999;
    float4 grad_results_0 = rsqrt(grad_x0 * grad_x0 + grad_y0 * grad_y0 + grad_z0 * grad_z0) * (float2(Pf.x, Pf_min1.x).xyxy * grad_x0 + float2(Pf.y, Pf_min1.y).xxyy * grad_y0 + Pf.zzzz * grad_z0);
    float4 grad_results_1 = rsqrt(grad_x1 * grad_x1 + grad_y1 * grad_y1 + grad_z1 * grad_z1) * (float2(Pf.x, Pf_min1.x).xyxy * grad_x1 + float2(Pf.y, Pf_min1.y).xxyy * grad_y1 + Pf_min1.zzzz * grad_z1);


    #if 1 
    //	Classic Perlin Interpolation 
    float3 blend = Interpolation_C2(Pf);
    float4 res0 = lerp(grad_results_0, grad_results_1, blend.z);
    float4 blend2 = float4(blend.xy, float2(1.0 - blend.xy));
    float final = dot(res0, blend2.zxzx * blend2.wwyy);
    final *= 1.1547005383792515290182975610039;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/sqrt(0.75) 
    return final;
    #else 
    //	Classic Perlin Surflet 
    //	http://briansharpe.wordpress.com/2012/03/09/modifications-to-classic-perlin-noise/ 
    Pf *= Pf;
    Pf_min1 *= Pf_min1;
    float4 vecs_len_sq = float4(Pf.x, Pf_min1.x, Pf.x, Pf_min1.x) + float4(Pf.yy, Pf_min1.yy);
    float final = dot(Falloff_Xsq_C2(min(float4(1.0), vecs_len_sq + Pf.zzzz)), grad_results_0) + dot(Falloff_Xsq_C2(min(float4(1.0), vecs_len_sq + Pf_min1.zzzz)), grad_results_1);
    final *= 2.3703703703703703703703703703704;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/cube(0.75) 
    return final;
    #endif 


    #else 
    // 
    //	improved noise. 
    //	requires 1 random value per point.  Will run faster than classic noise if a slow hashing function is used 
    // 

    //	calculate the hash. 
    //	( various hashing methods listed in order of speed ) 
    float4 hash_lowz, hash_highz;
    FAST32_hash_3D(Pi, hash_lowz, hash_highz);
    //BBS_hash_3D( Pi, hash_lowz, hash_highz ); 
    //SGPP_hash_3D( Pi, hash_lowz, hash_highz ); 

    // 
    //	"improved" noise using 8 corner gradients.  Faster than the 12 mid-edge point method. 
    //	Ken mentions using diagonals like this can cause "clumping", but we'll live with that. 
    //	[1,1,1]  [-1,1,1]  [1,-1,1]  [-1,-1,1] 
    //	[1,1,-1] [-1,1,-1] [1,-1,-1] [-1,-1,-1] 
    // 
    hash_lowz -= 0.5;
    float4 grad_results_0_0 = float2(Pf.x, Pf_min1.x).xyxy * sign(hash_lowz);
    hash_lowz = abs(hash_lowz) - 0.25;
    float4 grad_results_0_1 = float2(Pf.y, Pf_min1.y).xxyy * sign(hash_lowz);
    float4 grad_results_0_2 = Pf.zzzz * sign(abs(hash_lowz) - 0.125);
    float4 grad_results_0 = grad_results_0_0 + grad_results_0_1 + grad_results_0_2;

    hash_highz -= 0.5;
    float4 grad_results_1_0 = float2(Pf.x, Pf_min1.x).xyxy * sign(hash_highz);
    hash_highz = abs(hash_highz) - 0.25;
    float4 grad_results_1_1 = float2(Pf.y, Pf_min1.y).xxyy * sign(hash_highz);
    float4 grad_results_1_2 = Pf_min1.zzzz * sign(abs(hash_highz) - 0.125);
    float4 grad_results_1 = grad_results_1_0 + grad_results_1_1 + grad_results_1_2;

    //	blend the gradients and return 
    float3 blend = Interpolation_C2(Pf);
    float4 res0 = mix(grad_results_0, grad_results_1, blend.z);
    float4 blend2 = float4(blend.xy, float2(1.0 - blend.xy));
    return dot(res0, blend2.zxzx * blend2.wwyy) * (2.0 / 3.0);	//	(optionally) mult by (2.0/3.0) to scale to a strict -1.0->1.0 range 
    #endif 


}


//
//	Perlin Noise 2D  ( gradient noise )
//	Return value range of -1.0->1.0
//	http://briansharpe.files.wordpress.com/2011/11/perlinsample.jpg
//
float Perlin2D(float2 P)
{
    //	establish our grid cell and unit position
    float2 Pi = floor(P);
    float4 Pf_Pfmin1 = P.xyxy - float4(Pi, Pi + 1.0);

    #if 1
    //
    //	classic noise looks much better than improved noise in 2D, and with an efficent hash function runs at about the same speed.
    //	requires 2 random numbers per point.
    //

    //	calculate the hash.
    //	( various hashing methods listed in order of speed )
    float4 hash_x, hash_y;
    FAST32_hash_2D(Pi, hash_x, hash_y);
    //SGPP_hash_2D( Pi, hash_x, hash_y );

    //	calculate the gradient results
    float4 grad_x = hash_x - 0.49999;
    float4 grad_y = hash_y - 0.49999;
    float4 grad_results = rsqrt(grad_x * grad_x + grad_y * grad_y) * (grad_x * Pf_Pfmin1.xzxz + grad_y * Pf_Pfmin1.yyww);

    #if 1
    //	Classic Perlin Interpolation
    grad_results *= 1.4142135623730950488016887242097;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/sqrt(0.5)
    float2 blend = Interpolation_C2(Pf_Pfmin1.xy);
    float4 blend2 = float4(blend, float2(1.0 - blend));
    return dot(grad_results, blend2.zxzx * blend2.wwyy);
    #else
    //	Classic Perlin Surflet
    //	http://briansharpe.wordpress.com/2012/03/09/modifications-to-classic-perlin-noise/
    grad_results *= 2.3703703703703703703703703703704;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/cube(0.75)
    float4 vecs_len_sq = Pf_Pfmin1 * Pf_Pfmin1;
    vecs_len_sq = vecs_len_sq.xzxz + vecs_len_sq.yyww;
    return dot(Falloff_Xsq_C2(min(float4(1.0), vecs_len_sq)), grad_results);
    #endif

    #else
    //
    //	2D improved perlin noise.
    //	requires 1 random value per point.
    //	does not look as good as classic in 2D due to only a small number of possible cell types.  But can run a lot faster than classic perlin noise if the hash function is slow
    //

    //	calculate the hash.
    //	( various hashing methods listed in order of speed )
    float4 hash = FAST32_hash_2D(Pi);
    //float4 hash = BBS_hash_2D( Pi );
    //float4 hash = SGPP_hash_2D( Pi );
    //float4 hash = BBS_hash_hq_2D( Pi );

    //
    //	evaulate the gradients
    //	choose between the 4 diagonal gradients.  ( slightly slower than choosing the axis gradients, but shows less grid artifacts )
    //	NOTE:  diagonals give us a nice strict -1.0->1.0 range without additional scaling
    //	[1.0,1.0] [-1.0,1.0] [1.0,-1.0] [-1.0,-1.0]
    //
    hash -= 0.5;
    float4 grad_results = Pf_Pfmin1.xzxz * sign(hash) + Pf_Pfmin1.yyww * sign(abs(hash) - 0.25);

    //	blend the results and return
    float2 blend = Interpolation_C2(Pf_Pfmin1.xy);
    float4 blend2 = float4(blend, float2(1.0 - blend));
    return dot(grad_results, blend2.zxzx * blend2.wwyy);

    #endif

}

//	convert a 0.0->1.0 sample to a -1.0->1.0 sample weighted towards the extremes
float4 Cellular_weight_samples(float4 samples)
{
    samples = samples * 2.0 - 1.0;
    //return (1.0 - samples * samples) * sign(samples);	// square
    return (samples * samples * samples) - sign(samples);	// cubic (even more variance)
}

//
//	Cellular Noise 2D
//	Based off Stefan Gustavson's work at http://www.itn.liu.se/~stegu/GLSL-cellular
//	http://briansharpe.files.wordpress.com/2011/12/cellularsample.jpg
//
//	Speed up by using 2x2 search window instead of 3x3
//	produces a range of 0.0->1.0
//
float Cellular2D(float2 P)
{
    //	establish our grid cell and unit position
    float2 Pi = floor(P);
    float2 Pf = P - Pi;

    //	calculate the hash.
    //	( various hashing methods listed in order of speed )
    float4 hash_x, hash_y;
    FAST32_hash_2D(Pi, hash_x, hash_y);
    //SGPP_hash_2D( Pi, hash_x, hash_y );

    //	generate the 4 random points
    #if 1
    //	restrict the random point offset to eliminate artifacts
    //	we'll improve the variance of the noise by pushing the points to the extremes of the jitter window
    const float JITTER_WINDOW = 0.25;	// 0.25 will guarentee no artifacts.  0.25 is the intersection on x of graphs f(x)=( (0.5+(0.5-x))^2 + (0.5-x)^2 ) and f(x)=( (0.5+x)^2 + x^2 )
    hash_x = Cellular_weight_samples(hash_x) * JITTER_WINDOW + float4(0.0, 1.0, 0.0, 1.0);
    hash_y = Cellular_weight_samples(hash_y) * JITTER_WINDOW + float4(0.0, 0.0, 1.0, 1.0);
    #else
    //	non-weighted jitter window.  jitter window of 0.4 will give results similar to Stefans original implementation
    //	nicer looking, faster, but has minor artifacts.  ( discontinuities in signal )
    const float JITTER_WINDOW = 0.4;
    hash_x = hash_x * JITTER_WINDOW * 2.0 + float4(-JITTER_WINDOW, 1.0 - JITTER_WINDOW, -JITTER_WINDOW, 1.0 - JITTER_WINDOW);
    hash_y = hash_y * JITTER_WINDOW * 2.0 + float4(-JITTER_WINDOW, -JITTER_WINDOW, 1.0 - JITTER_WINDOW, 1.0 - JITTER_WINDOW);
    #endif

    //	return the closest squared distance
    float4 dx = Pf.xxxx - hash_x;
    float4 dy = Pf.yyyy - hash_y;
    float4 d = dx * dx + dy * dy;
    d.xy = min(d.xy, d.zw);
    return min(d.x, d.y) * (1.0 / 1.125);	//	scale return value from 0.0->1.125 to 0.0->1.0  ( 0.75^2 * 2.0  == 1.125 )
}