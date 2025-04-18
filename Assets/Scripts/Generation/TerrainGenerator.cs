using UnityEngine;

public static class TerrainGenerator {
    public static float[,] Generate( int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, float falloffExponent, float falloffBlend ) {
        float[,] heightMap = new float[width, height];
        for( int y = 0; y < height; y++ ) {
            for( int x = 0; x < width; x++ ) {
                float noise = Noise.GeneratePerlinNoise( x / (float)( width ), y / (float)( height ), scale, octaves, persistence, lacunarity, seed );
                float falloff = Noise.EvaluateFalloff( x, y, width, height, falloffExponent, falloffBlend );
                float finalHeight = Mathf.Clamp01( noise - falloff );

                heightMap[x, y] = finalHeight;
            }
        }

        return heightMap;
    }
}