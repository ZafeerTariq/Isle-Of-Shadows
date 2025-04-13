using UnityEngine;

public static class Noise {
    public static float GeneratePerlinNoise( float x, float y, float scale, float octaves, float persistence, float lacunarity, int seed ) {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;
        float maxAmplitude = 0;

        System.Random prng = new System.Random( seed );
        float offsetX = prng.Next( -100000, 100000 );
        float offsetY = prng.Next( -100000, 100000 );

        for( int i = 0; i < octaves; i++ ) {
            float sampleX = ( x * scale * frequency ) + offsetX;
            float sampleY = ( y * scale * frequency ) + offsetY;

            float perlinValue = Mathf.PerlinNoise( sampleX, sampleY ) * 2f - 1f; // convert to [-1, 1] range
            noiseHeight += perlinValue * amplitude;

            maxAmplitude += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return ( noiseHeight / maxAmplitude + 1f ) / 2f; // Normalize to [0, 1]
    }

    public static float EvaluateFalloff( float x, float y, int width, int height, float falloffExponent, float blend ) {
        float nx = ( x / (float)width ) * 2 - 1;
        float ny = ( y / (float)height ) * 2 - 1;

        float circleDistance = Mathf.Sqrt( nx * nx + ny * ny );
        float squareDistance = Mathf.Max(Mathf.Abs(nx), Mathf.Abs(ny));
        float distance = Mathf.Lerp( circleDistance, squareDistance, blend );

        distance /= 1.2f;

        float falloff = Mathf.Pow( distance, falloffExponent );
        return Mathf.Clamp01( falloff );
    }
}