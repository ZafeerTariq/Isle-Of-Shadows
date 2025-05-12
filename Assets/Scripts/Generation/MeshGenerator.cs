using UnityEngine;

[RequireComponent( typeof( MeshFilter ), typeof( MeshRenderer ) )]
public class MeshGenerator : MonoBehaviour {
	public int xSize = 20;
	public int zSize = 20;
	public NoiseDataScriptableObject noiseData;

	public float falloffExponent;
	public float falloffBlend;
	public float heightMultiplier;

	public GameObject tree;
	public GameObject stone;
	public RegionGenerator region;

	private Color waterColor    = new Color( 0f, 0.349f, 0.702f );
	private Color sandColor     = new Color( 0.918f, 0.745f, 0.459f );
	private Color treeColor     = new Color( 0.1f, 0.45f, 0.1f );
	private Color grassColor    = new Color( 0.2f, 0.5f, 0.2f );
	private Color stoneColor    = new Color( 0.5f, 0.5f, 0.5f );

	public Gradient colorGradient;

	void OnEnable() {
		colorGradient = new Gradient();

		GradientColorKey[] colorKeys = new GradientColorKey[5];
		colorKeys[0].color	= waterColor;
		colorKeys[0].time	= 0.0f;

		colorKeys[1].color	= sandColor;
		colorKeys[1].time	= 0.1f;

		colorKeys[2].color	= grassColor;
		colorKeys[2].time	= 0.5f;

		colorKeys[3].color	= treeColor;
		colorKeys[3].time	= 0.75f;

		colorKeys[4].color	= stoneColor;
		colorKeys[4].time	= 1.0f;

		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
		alphaKeys[0].alpha = 1.0f;
		alphaKeys[0].time = 0.0f;
		alphaKeys[1].alpha = 1.0f;
		alphaKeys[1].time = 1.0f;

		colorGradient.SetKeys( colorKeys, alphaKeys );

		Generate();
	}

	private void Generate() {
		Mesh mesh = new Mesh { name = "Procedural Terrain" };

		// for meshes which have more that 65,000 vertices
		// mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		Vector3[] vertices  = new Vector3[( xSize + 1 ) * ( zSize + 1 )];
		int[] triangles     = new int[xSize * zSize * 6];
		Vector2[] uv        = new Vector2[vertices.Length];
		Vector4[] tangents  = new Vector4[vertices.Length];
		Color[] colorMap	= new Color[( xSize + 1 ) * ( zSize + 1 )];
		Vector4 tangent     = new Vector4( 1, 0, 0, 1 );

		float[,] heightMap = TerrainGenerator.Generate( xSize + 1, zSize + 1, noiseData, falloffExponent, falloffExponent );
		RegionType[,] regionMap = region.Generate( heightMap );

		for( int i = 0, z = 0; z < zSize + 1; z++ ) {
			for( int x = 0; x < xSize + 1; x++ ) {
				float y = heightMap[x, z] * heightMultiplier;

				vertices[i] = new Vector3( x, y, z );
				uv[i] = new Vector2( (float)x / xSize, (float)z / zSize );
				tangents[i] = tangent;

				RegionType region = regionMap[x, z];
				if( region == RegionType.Trees ) {
					if( Random.Range( 0f, 1f ) < 0.1f ) {
						Instantiate( tree, new Vector3( x, y, z ), Quaternion.identity, transform );
					}
				}
				else if( region == RegionType.Stone ) {
					if( Random.Range( 0f, 1f ) < 0.1f ) {
						Instantiate( stone, new Vector3( x, y, z ), Quaternion.identity, transform );
					}
				}

				colorMap[i] = colorGradient.Evaluate( heightMap[x, z] );
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

		GameObject wall = new GameObject( "wall" );

		BoxCollider bottom = wall.AddComponent<BoxCollider>();
		bottom.center = new Vector3( (xSize + 1) / 2f, 0, 0 );
		bottom.size 	= new Vector3( xSize + 1, 35, 0 );

		BoxCollider top = wall.AddComponent<BoxCollider>();
		top.center = new Vector3( (xSize + 1) / 2f, 0, zSize + 1 );
		top.size 	= new Vector3( xSize + 1, 35, 0 );

		BoxCollider left = wall.AddComponent<BoxCollider>();
		left.center = new Vector3( 0, 0, (zSize + 1) / 2f );
		left.size 	= new Vector3( 0, 35, zSize + 1 );

		BoxCollider right = wall.AddComponent<BoxCollider>();
		right.center = new Vector3( xSize + 1, 0, (zSize + 1) / 2f );
		right.size 	= new Vector3( 0, 35, zSize + 1 );
	}
}
