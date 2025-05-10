using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundGenerator : MonoBehaviour {
	public int width  = 1920;
	public int height = 1080;
	public NoiseDataScriptableObject noiseData;

	public Image image;

	public Color water;
	public Color ground;
	public Color rock;
	public Color snow;

	private float[,] noiseMap;
	private Color[] colorMap;

	void Start() {
		noiseData.seed = Random.Range( 0, int.MaxValue );
		Generate();
	}

	void Update() {
		if( Input.GetKey( KeyCode.R ) ) {
			Generate();
		}
	}

	void Generate() {
				noiseMap = new float[height, width];
		colorMap = new Color[width * height];

		for( int y = 0, i = 0; y < height; y++ ) {
			for( int x = 0; x < width; x++ ) {
				noiseMap[y, x] = Noise.GeneratePerlinNoise(
					x / (float)width, y / (float)height,
					noiseData.scale,
					noiseData.octaves,
					noiseData.persistence,
					noiseData.lacunarity,
					noiseData.seed
				);

				if( noiseMap[y, x] < 0.3f )
					colorMap[i] = water;
				else if( noiseMap[y, x] < 0.6f )
					colorMap[i] = ground;
				else if ( noiseMap[y, x]  < 0.8f )
					colorMap[i] = rock;
				else
					colorMap[i] = snow;
				i++;
			}
		}

		Texture2D texture = TextureGenerator.TextureFromColorMap( colorMap, width, height );
		Sprite sprite = Sprite.Create( texture, new Rect( 0, 0, width, height ), new Vector2( 0.5f, 0.5f ) );
		image.sprite = sprite;
	}
}