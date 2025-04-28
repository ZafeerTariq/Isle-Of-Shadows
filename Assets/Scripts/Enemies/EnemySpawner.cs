using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public int spawnCount;
	public List<GameObject> prefabs;
	public float timeBetweenSpawns;
	public MeshGenerator meshGenerator;

	private float time = 0f;
	private int mapWidth;
	private int mapHeight;

	void Start() {
		mapWidth  = meshGenerator.xSize;
		mapHeight = meshGenerator.zSize;
	}

	void Update() {
		time += Time.deltaTime;
		if( time >= timeBetweenSpawns ) {
			time = 0f;
			for( int i = 0; i < spawnCount; i++ ) {
				Vector3 position = new Vector3( Random.Range( 0f, mapWidth ), 5, Random.Range( 0f, mapHeight ) );
				GameObject spawn = Instantiate( prefabs[Random.Range( 0, prefabs.Count )], position, Quaternion.identity, transform );
			}
		}
	}
}
