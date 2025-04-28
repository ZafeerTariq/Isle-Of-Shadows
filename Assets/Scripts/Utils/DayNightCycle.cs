using UnityEngine;

public class DayNightCycle : MonoBehaviour {
	public Light sun;
	public int dayLength = 10;

	public Color start;
	public Color end;
    public AnimationCurve lightIntensityOverDay;

	private float time = 0f;

	void Update() {
		time += Time.deltaTime / dayLength;
		if( time >= 1f ) {
			time = 0f;
		}

		sun.transform.rotation = Quaternion.Euler( (time * 360f) - 90f, 170f, 0f );

		sun.color = Color.Lerp( start, end, time );
        sun.intensity = lightIntensityOverDay.Evaluate( time );

        RenderSettings.ambientLight = sun.color * 0.5f;
	}
}
