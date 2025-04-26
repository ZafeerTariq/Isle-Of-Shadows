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

	public GameObject tree;
	public GameObject stone;
	public RegionGenerator region;

	private Color[] colorMap;
	private Color waterColor    = new Color( 0f, 0.349f, 0.702f );
	private Color sandColor     = new Color( 0.918f, 0.745f, 0.459f );
	private Color treeColor     = new Color( 0.1f, 0.45f, 0.1f );
	private Color grassColor    = new Color( 0.2f, 0.5f, 0.2f );
	private Color stoneColor    = new Color( 0.5f, 0.5f, 0.5f );

	void OnEnable() {
		Generate();
	}

	void Update() {
		if( Input.GetKeyDown( KeyCode.R ) ) {
			foreach( Transform child in transform ) {
				Destroy( child.gameObject );
			}
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

		colorMap = new Color[( xSize + 1 ) * ( zSize + 1 )];
		float[,] heightMap = TerrainGenerator.Generate( xSize + 1, zSize + 1, seed, scale, octaves, persistence, lacunarity, falloffExponent, falloffExponent );
		RegionType[,] regionMap = region.Generate( heightMap );

		for( int i = 0, z = 0; z < zSize + 1; z++ ) {
			for( int x = 0; x < xSize + 1; x++ ) {
				float y = heightMap[x, z] * heightMultiplier;

				vertices[i] = new Vector3( x, y, z );
				uv[i] = new Vector2( (float)x / xSize, (float)z / zSize );
				tangents[i] = tangent;

                Color vertexColor;
                if( heightMap[x, z] < 0.1f )
                    vertexColor = waterColor;
                else
                    vertexColor = sandColor;

				RegionType region = regionMap[x, z];
				if( region == RegionType.Trees ) {
					if( Random.Range( 0f, 1f ) < 0.1f ) {
						Instantiate( tree, new Vector3( x, y, z ), Quaternion.identity, transform );
					}
					vertexColor = treeColor;
				}
				else if( region == RegionType.Stone ) {
					if( Random.Range( 0f, 1f ) < 0.1f ) {
						Instantiate( stone, new Vector3( x, y, z ), Quaternion.identity, transform );
					}
					vertexColor = stoneColor;
				}
				else if( region == RegionType.Grass ) {
					vertexColor = grassColor;
				}

				colorMap[i] = vertexColor;
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
		mesh.colors = colorMap;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;

		// Texture2D texture = TextureGenerator.TextureFromColorMap( colorMap, xSize + 1, zSize + 1 );
		// GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
	}
}
