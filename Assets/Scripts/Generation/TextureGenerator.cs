using UnityEngine;

public static class TextureGenerator {
	public static Texture2D TextureFromColorMap( Color[] colorMap, int width, int height ) {
		Texture2D texture = new Texture2D( width, height );
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Trilinear;
		texture.SetPixels( colorMap );
		texture.Apply();
		return texture;
	}
}