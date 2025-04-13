using UnityEngine;

[RequireComponent( typeof( MeshFilter ), typeof( MeshRenderer ) )]
public class MeshGenerator : MonoBehaviour {
    public int xSize = 20;
    public int zSize = 20;
    public int seed;
    public float scale;
    public int octaves;
    public float persistence;
    public float lacunarity;

    public float falloffExponent;
    public float falloffBlend;
    public float heightMultiplier;

    public Material material;

    void OnEnable() {
        Generate();
    }

    void Update() {
        if( Input.GetKeyDown( KeyCode.R ) ) {
            Generate();
        }
    }

    private void Generate() {
        Mesh mesh = new Mesh { name = "Procedural Terrain" };

        // for meshes which have more that 65,000 vertices
        // mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        Vector3[] vertices  = new Vector3[( xSize + 1 ) * ( zSize + 1 )];
        int[] triangles     = new int[xSize * zSize * 6];
        Vector2[] uv        = new Vector2[vertices.Length];
        Vector4[] tangents  = new Vector4[vertices.Length];
        Vector4 tangent     = new Vector4( 1, 0, 0, 1 );

        for( int i = 0, z = 0; z < zSize + 1; z++ ) {
            for( int x = 0; x < xSize + 1; x++ ) {
                float noise = Noise.GeneratePerlinNoise( x / (float)( xSize + 1 ), z / (float)( zSize + 1 ), scale, octaves, persistence, lacunarity, seed );
                float falloff = Noise.EvaluateFalloff( x, z, xSize + 1, zSize + 1, falloffExponent, falloffBlend );
                float finalHeight = Mathf.Clamp01( noise - falloff );
                float y = finalHeight * heightMultiplier;

                material.color = Color.Lerp( Color.black, Color.white, y );

                vertices[i] = new Vector3( x, y, z );
                uv[i] = new Vector2( (float)x / xSize, (float)z / zSize );
                tangents[i] = tangent;
                i++;
            }
        }

        for( int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++ ) {
			for( int x = 0; x < xSize; x++, ti += 6, vi++ ) {
                triangles[ti] = vi;
				triangles[ti + 1] = vi + xSize + 2;
				triangles[ti + 2] = vi + 1;
				triangles[ti + 3] = vi;
                triangles[ti + 4] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.tangents = tangents;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
