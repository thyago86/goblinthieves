using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GridInfo : SingletonMonoBehaviour<GridInfo> {

	public Tilemap Chao;
	public Tilemap Parede;
	public Tilemap Fog;
	public Tilemap Doors;

	public List<BoundsInt> mapBounds = new List<BoundsInt>();

	public List<Vector3Int> LitTiles = new List<Vector3Int>();
	public Dictionary<Vector3Int,int> wall_cost;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
