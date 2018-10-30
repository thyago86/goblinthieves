using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler{

	public Dictionary<Tilemap,int> map_cost;

	public static Vector3Int[] getTilesAround(Vector3Int pos, Tilemap Map){
		List<Vector3Int> Result = new List<Vector3Int>();

		int CellX = (int) Map.cellSize.x;
		int CellY = (int) Map.cellSize.y;

		Vector3Int CellSize = new Vector3Int(CellX,CellY,0);

		Vector3Int checkUp = new Vector3Int(pos.x,pos.y+CellSize.y,0);
		Vector3Int checkDown = new Vector3Int(pos.x,pos.y-CellSize.y,0);
		Vector3Int checkLeft = new Vector3Int(pos.x-CellSize.x,pos.y,0);
		Vector3Int checkRight = new Vector3Int(pos.x+CellSize.x,pos.y,0);

		Vector3Int[] orbitCells = {checkUp, checkDown, checkLeft, checkRight};

		for(int i=0; i<orbitCells.Length;i++){
			if(Map.HasTile(orbitCells[i])){
				Result.Add(orbitCells[i]);
			}
		}
			
		return Result.ToArray();
	}

	public static List<Vector3Int> getAllTilesOnMap(Tilemap tileMap){
		List<Vector3Int> Result = new List<Vector3Int>();
		
		for (int i = tileMap.cellBounds.xMin; i < tileMap.cellBounds.xMax; i++){
            for (int j = tileMap.cellBounds.yMin; j < tileMap.cellBounds.yMax; j++){
				Vector3Int pos = new Vector3Int(i,j,0);
				if(tileMap.HasTile(pos)){
					Result.Add(pos);
				}
			}
		}
		return Result;
	}

	public static List<Vector3Int> getAllTilesOnAreaOnMap(Tilemap tileMap, Vector3Int originCell, int areaSizeH, int areaSizeV){
		List<Vector3Int> Result = new List<Vector3Int>();

		BoundsInt Area = new BoundsInt(originCell,new Vector3Int(areaSizeH,areaSizeV,1));
		Area.SetMinMax(new Vector3Int(originCell.x-areaSizeH,originCell.y-areaSizeV,-1),new Vector3Int(originCell.x+areaSizeH,originCell.y+areaSizeV,1));
		
		foreach(var pos in Area.allPositionsWithin){
			if(tileMap.HasTile(pos)){
				Result.Add(pos);
			}
		}
		
		return Result;
	}

	public static Vector3Int[] getAllTilesVisibilityTilesAround(Tilemap FloorMap,Tilemap ObstacleMap, Vector3Int originCell, int areaSize){
		List<Vector3Int> Result = new List<Vector3Int>();

		int CellX = (int) FloorMap.cellSize.x;
		int CellY = (int) FloorMap.cellSize.y;
		/* 
			BoundsInt Area = new BoundsInt(originCell,new Vector3Int(areaSizeH,areaSizeV,1));
			Area.SetMinMax(new Vector3Int(originCell.x-areaSizeH,originCell.y-areaSizeV,-1),new Vector3Int(originCell.x+areaSizeH+1,originCell.y+areaSizeV+1,1));

			foreach(var pos in Area.allPositionsWithin){
				if(tileMap.HasTile(pos) && !ObstacleMap.HasTile(pos)){
					Result.Add(pos);
				}
			}
		*/
		/* 
			Queue<Vector3Int> Fronteira = new Queue<Vector3Int>();
			Fronteira.Enqueue(originCell);
			Result.Add(originCell);

			while(Fronteira.Count > 0){
				Vector3Int Current = Fronteira.Dequeue();
				if(Vector3Int.Distance(originCell,Current) > FloorMap.cellSize.magnitude*areaSize){
					break;
				}
				Vector3Int[] CurrentNeighbors = getTilesAround(Current, FloorMap);
				foreach (Vector3Int n in CurrentNeighbors){
					if(FloorMap.HasTile(n) && !Result.Contains(n)){
						//Debug.Log(n.ToString());
						Result.Add(n);
						if(!ObstacleMap.HasTile(n)){
							Fronteira.Enqueue(n);
						}
					}
				}
			}
		*/

		
		
		
		return Result.ToArray();
	}

	public static void SortUsingManhattanDistance(Vector3Int[] array, Vector3Int goal){
		
		for(int i=0;i<array.Length-1;i++){
			int distanceCurrent = Mathf.Abs((array[i].x - goal.x)) + Mathf.Abs((array[i].y - goal.y));
			
			for(int j=i+1;j<array.Length;j++){
				int distanceCheck = Mathf.Abs((array[j].x - goal.x)) + Mathf.Abs((array[j].y - goal.y));
				if(distanceCheck < distanceCurrent){
					Vector3Int temp = array[j];
					array[j] = array[i];
					array[i] = temp;
				}
			}
		}
	}

	public static Vector3Int[] GetCellsDir(Tilemap Map, Vector2 Dir, int maxDist, bool Passthrough){
		
	}

	public static Vector3Int[] AStar(Tilemap FloorMap,Tilemap ObstacleMap, Vector3Int originCell, Vector3Int goalCell){
		List<Vector3Int> Result = new List<Vector3Int>();
		Dictionary<Vector3Int,Vector3Int> came_from = new Dictionary<Vector3Int, Vector3Int>();

		Queue<Vector3Int> Fronteira = new Queue<Vector3Int>();
		Fronteira.Enqueue(originCell);
		Result.Add(originCell);

		while(Fronteira.Count > 0){
			Vector3Int Current = Fronteira.Dequeue();

			Vector3Int[] CurrentNeighbors = getTilesAround(Current, FloorMap);
			SortUsingManhattanDistance(CurrentNeighbors,goalCell);

			for(int i=0;i<CurrentNeighbors.Length;i++){
				if(FloorMap.HasTile(CurrentNeighbors[i]) && !Result.Contains(CurrentNeighbors[i])){
					//Debug.Log(n.ToString());
					Fronteira.Enqueue(CurrentNeighbors[i]);
					//if(){
						Result.Add(CurrentNeighbors[i]);
					//}
				}
			}
		}

		return Result.ToArray();
	}

}
