﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class GenerateClusterRoom : MonoBehaviour {
	[Range(0,100)]
	public int iniChance;
	
	[Range(1,8)]
	public int birthLimit;
	[Range(1,8)]
	public int deathLimit;
	[Range(1,10)]
	public int numR;
	int count = 0;
	int[,] terrainMap;
	public Vector3Int tmapSize;
	public Tilemap topMap;
	public Tilemap botMap;
	public Tile topTile;
	public Tile HideSpotTile;

	int height;
	int width;

	public void doSim(int numR){
		clearMap(false);
		width = tmapSize.x;
		height = tmapSize.y;

		if(terrainMap == null){
			terrainMap = new int[width, height];
			initPos();
		}
		for(int i = 0; i < numR; i++){
			terrainMap = genTilePos(terrainMap);
		}
		for (int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				if(terrainMap[x,y] == 1){
					if(GridInfo.instance.Chao.HasTile(new Vector3Int(-x + width/2, -y + height/2, 0)) && !GridInfo.instance.Exits.HasTile(new Vector3Int(-x + width/2, -y + height/2, 0))){
						topMap.SetTile(new Vector3Int(-x + width/2, -y + height/2, 0), topTile);
						botMap.SetTile(new Vector3Int(-x + width/2, -y + height/2, 0), topTile);
					}
				}
			}
		}
	}

	public int[,] genTilePos(int [,] oldMap){
		int[,] newMap = new int[width, height];
		int neigh;
		BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);
		for(int x = 0; x<width; x++){
			for(int y = 0; y<height; y++){
				neigh = 0;
				foreach(var b in myB.allPositionsWithin){
					if(b.x == 0 && b.y == 0) continue; //laterais, sem vizinho, continue
					if(x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y+b.y < height){
						neigh += oldMap[x + b.x, y + b.y];
					}
				}
				if(oldMap[x,y] == 1){
					if (neigh < deathLimit) newMap[x,y] = 0;
					else{
						newMap[x,y] = 1;
					}
				}
				if(oldMap[x,y] == 0){
					if(neigh > birthLimit) newMap[x, y] = 1;
					else{
						newMap[x, y] = 0;
					}
				}
			}
			
		}
		

		return newMap;
	}
	public void initPos(){
		for(int x = 0; x<width; x++){
			for(int y = 0; y<height; y++){
				terrainMap[x,y] = Random.Range(1, 101) < iniChance ? 1: 0;
			}
		}
	}

	public void clearMap(bool complete){
		topMap.ClearAllTiles();
		//botMap.ClearAllTiles();
		if(complete){
			terrainMap = null;
		}
	}
	
	public void PlaceHideAndTorchesBFS(){

		Queue<Vector3Int> Fronteira = new Queue<Vector3Int>();
		List<Vector3Int> Visited = new List<Vector3Int>();

		Tilemap FloorMap = GridInfo.instance.Chao;
		Tilemap ObstacleMap = GridInfo.instance.Parede;
		Tilemap HideS = GridInfo.instance.HideSpots;


		Vector3Int origin = new Vector3Int(0,0,0);
		Fronteira.Enqueue(origin);

		while(Fronteira.Count > 0){
			
			Vector3Int Current = Fronteira.Dequeue();
			Visited.Add(Current);
			
			Vector3Int[] CurrentNeighbors = GridHandler.getTilesAround(Current, GridInfo.instance.Chao);
			Vector3Int[] WallsAround = GridHandler.getTilesAround(Current, GridInfo.instance.Parede);

			if(WallsAround.Length == 3){
				FloorMap.SetTileFlags(Current,TileFlags.None);
				FloorMap.SetColor(Current,Color.blue);
			}

			foreach(Vector3Int neighbour in CurrentNeighbors){
				if(FloorMap.HasTile(neighbour) && !ObstacleMap.HasTile(neighbour) && !Visited.Contains(neighbour)){
					Fronteira.Enqueue(neighbour);
				}
			}
		}

	}

	public void PlaceHideSpots(){

		Queue<Vector3Int> ToCheckQueue = new Queue<Vector3Int>();

		foreach(Vector3Int wallPos in GridInfo.instance.Parede.cellBounds.allPositionsWithin){
			Vector3Int[] posAroundWall = GridHandler.getTilesAround(wallPos, GridInfo.instance.Chao);
			foreach(Vector3Int t in posAroundWall){
				ToCheckQueue.Enqueue(t);
			}
		}

		while(ToCheckQueue.Count > 0){
			Vector3Int checking = ToCheckQueue.Dequeue();

			Vector3Int[] wallsAround = GridHandler.getTilesAround(checking, GridInfo.instance.Parede);

			if(wallsAround.Length == 3){
				if(!GridInfo.instance.Parede.HasTile(checking)){
					GridInfo.instance.HideSpots.SetTile(checking, HideSpotTile);
				}
				
			}
		}
	}

	public void ClearTreasureSpot(){
		GameObject[] AllTreasures = GameObject.FindGameObjectsWithTag("Treasure");

		foreach(GameObject t in AllTreasures){
			Vector3Int tPos = GridInfo.instance.Chao.WorldToCell(t.transform.position);
			GridInfo.instance.Parede.SetTile(tPos,null);
		}

	}

	public void ClearTorchSpot(){
		GameObject[] AllTreasures = GameObject.FindGameObjectsWithTag("Interactable");

		foreach(GameObject t in AllTreasures){
			Vector3Int tPos = GridInfo.instance.Chao.WorldToCell(t.transform.position);
			GridInfo.instance.Parede.SetTile(tPos,null);
		}

	}

	void Update () {
		if(Input.GetMouseButtonDown(0)){
			//doSim(numR);
		}
		if(Input.GetMouseButtonDown(1)){
			//clearMap(true);  
		}
	}

	void Start(){
		doSim(numR);
		foreach(BoundsInt room in GridInfo.instance.mapBounds){
			GridInfo.instance.Parede.SetTile(new Vector3Int(Mathf.FloorToInt(room.center.x),Mathf.FloorToInt(room.center.y),0),null);
		}
		PlaceHideSpots();

		ClearTreasureSpot();
		ClearTorchSpot();
		//PlaceHideAndTorchesBFS();
	}
}
