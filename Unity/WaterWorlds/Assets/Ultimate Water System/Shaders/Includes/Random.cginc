inline float random(float2 p)
{
    float2 r = float2(23.14069263277926, 2.665144142690225);
    return frac(cos(dot(p, r)) * 123.0);
}

inline float gauss(float2 p)
{
    return sqrt(-2.0f * log(random(p))) * sin(3.14159265359f * 2.0 * random(p * -0.3241241));
}

inline float halfGauss(float2 p)
{
    return abs(sqrt(-2.0f * log(random(p))) * sin(3.14159265359f * 2.0 * random(p * -0.3241241)));
}