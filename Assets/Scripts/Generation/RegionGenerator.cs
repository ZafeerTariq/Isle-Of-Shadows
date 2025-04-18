using UnityEngine;

public enum RegionType {
    Trees,
    Stone,
    Grass,
    Water
}

public class RegionGenerator : MonoBehaviour {
    public int xSize = 20;
    public int zSize = 20;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;

    public GameObject tree;

    private Color[] colorMap;
    private Vector3 worldPos;

	void OnEnable() {
        worldPos = new Vector3( -xSize / 2f, 0, -zSize / 2f );
		Generate();
	}

    void Update() {
        if( Input.GetKeyDown( KeyCode.R ) ) {
            Generate();
        }
    }

    public RegionType[,] Generate() {
        RegionType[,] regionMap = new RegionType[xSize + 1, zSize + 1];
        colorMap = new Color[( xSize + 1 ) * ( zSize + 1 )];

        for( int i = 0, z = 0; z < zSize + 1; z++ ) {
            for( int x = 0; x < xSize + 1; x++ ) {
                float y = Noise.GeneratePerlinNoise( x / (float)( xSize + 1 ), z / (float)( zSize + 1 ), scale, octaves, persistence, lacunarity, seed );

                if( y < 0.33f ) {
                    colorMap[i] = Color.blue;
                    regionMap[x, z] = RegionType.Trees;
                }
                else if( y < 0.66f ) {
                    colorMap[i] = Color.green;
                    regionMap[x, z] = RegionType.Grass;
                }
                else {
                    colorMap[i] = Color.black;
                    regionMap[x, z] = RegionType.Stone;
                }

                i++;
            }
        }

        Texture2D texture = TextureGenerator.TextureFromColorMap( colorMap, xSize + 1, zSize + 1 );
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
        GetComponent<MeshRenderer>().transform.localScale = new Vector3( texture.width, 1, texture.height );
        return regionMap;
    }

    public RegionType[,] Generate( float[,] heightMap ) {
        RegionType[,] regionMap = new RegionType[xSize + 1, zSize + 1];

        for (int z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++) {
                float noise = Noise.GeneratePerlinNoise( x / (float)( xSize + 1 ), z / (float)( zSize + 1 ), scale, octaves, persistence, lacunarity, seed );
                float height = heightMap[x, z];

                float nx = (float)x / xSize * 2 - 1;
                float nz = (float)z / zSize * 2 - 1;
                float distanceFromCenter = Mathf.Sqrt(nx * nx + nz * nz);

                RegionType region = RegionType.Water;
                if( height < 0.3f ) {
                    region = RegionType.Water;
                }
                else if( height < 0.5f && noise < 0.66f ) {
                    region = RegionType.Grass;
                }
                else if( noise < 0.33f ) {
                    region = RegionType.Trees;
                }
                else if( distanceFromCenter < 0.3f && height > 0.6f ) {
                    region = RegionType.Stone;
                }

                regionMap[x, z] = region;
            }
        }

        return regionMap;
    }
}