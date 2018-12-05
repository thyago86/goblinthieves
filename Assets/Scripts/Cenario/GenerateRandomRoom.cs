using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateRandomRoom : MonoBehaviour {

	// Use this for initialization
	public TileBase wall;
	//public Tile floor;

	//public Vector3Int presentTile;
	//public Vector3Int lastTile;
	

	
	
	

	
	//checar se tem chão
	//checar se está dentro das dimensões da sala
	//checar se tem parede do lado
	int[] randomArray = new int[336];
	public BoundsInt area;
	

	//todas as salas menores tem os seguintes valores de tamanho: x=24 y=14
	/*
		Position:
		Sala norte -> limites x = -12 y = 9
		Sala esquerda -> limites x = -38 y = -7
		Sala direita -> limites x = 14 y = -7
		Sala sul -> limites x = -12 y= -23
	 */
	
	void Start(){
		
		
		TileBase[] randomTileArray = new TileBase[area.size.x * area.size.y * area.size.z];
		for (int i = 0; i< randomArray.Length; i++){
			randomArray[i] = Random.Range(0, 2);
			if(randomArray[i]==0){
				randomTileArray[i] = null;
			}else{
				randomTileArray[i] = wall;
			}
		}
		Tilemap tilemap = GetComponent<Tilemap>();
		tilemap.SetTilesBlock(area, randomTileArray);



		
	}
}
