using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler{

	public static Vector2 getMouseScreenPosition(){
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector2 mousePos = new Vector2(mouseRay.origin.x,mouseRay.origin.y);
		return mousePos;
	}

	public static Vector2 getTouchScreenPosition(Touch t){
		Ray touchRay = Camera.main.ScreenPointToRay(t.position);
		Vector2 touchPos = new Vector2(touchRay.origin.x,touchRay.origin.y);
		return touchPos;
	}

	public static Vector3 getRelevantTile(Vector3 pos, Tilemap tileMap){

		Vector3 finalPosition = pos;

		Vector3Int clickedCell = tileMap.WorldToCell(pos);

		if(tileMap.HasTile(clickedCell)){
			finalPosition = tileMap.GetCellCenterWorld(clickedCell);

		}
		else{
			Vector3Int CellUp = tileMap.WorldToCell(new Vector3(pos.x,pos.y+tileMap.cellSize.y,0));
			Vector3Int CellLeft = tileMap.WorldToCell(new Vector3(pos.x-tileMap.cellSize.x,pos.y,0));
			Vector3Int CellDown = tileMap.WorldToCell(new Vector3(pos.x,pos.y-tileMap.cellSize.y,0));
			Vector3Int CellRight = tileMap.WorldToCell(new Vector3(pos.x+tileMap.cellSize.x,pos.y,0));

			Vector3Int[] orbitCells = {CellUp, CellLeft, CellDown, CellRight};

			foreach(Vector3Int v in orbitCells){
				if(tileMap.HasTile(v)){
					finalPosition = tileMap.GetCellCenterWorld(v);
					break;
				}
			}
			
		}

		return finalPosition;

	}

	public static Vector3 getRelevantTile(Vector2 pos, Tilemap tileMap){

		Vector3 finalPosition = pos;

		Vector3Int clickedCell = tileMap.WorldToCell(pos);

		if(tileMap.HasTile(clickedCell)){
			
			finalPosition = tileMap.GetCellCenterWorld(clickedCell);

		}
		else{
			Vector3Int CellUp = tileMap.WorldToCell(new Vector3(pos.x,pos.y+tileMap.cellSize.y,0));
			Vector3Int CellLeft = tileMap.WorldToCell(new Vector3(pos.x-tileMap.cellSize.x,pos.y,0));
			Vector3Int CellDown = tileMap.WorldToCell(new Vector3(pos.x,pos.y-tileMap.cellSize.y,0));
			Vector3Int CellRight = tileMap.WorldToCell(new Vector3(pos.x+tileMap.cellSize.x,pos.y,0));

			Vector3Int[] orbitCells = {CellUp, CellLeft, CellDown, CellRight};

			foreach(Vector3Int v in orbitCells){
				if(tileMap.HasTile(v)){
					finalPosition = tileMap.GetCellCenterWorld(v);
					break;
				}
			}
			
		}

		return finalPosition;

	}

	public static bool checkBuildingSize(Vector3 pos, Tilemap tileMap, int sizeX, int sizeY){
		bool Result = true;

		//Vector3Int clickedCell = tileMap.WorldToCell(pos);

		for(int iX = 0; iX < sizeX; iX++){
			Vector3 posTem = new Vector3(pos.x + (-iX * tileMap.cellSize.x),pos.y + (iX * tileMap.cellSize.y),0);
			for(int iY = 0; iY < sizeY; iY++){
				Vector3Int checkCell = tileMap.WorldToCell(new Vector3(posTem.x+(iY * tileMap.cellSize.x),posTem.y+(iY * tileMap.cellSize.y),0));
				if(!tileMap.HasTile(checkCell)){
					//tileMap.SetTileFlags(checkCell, TileFlags.None);
					//tileMap.SetColor(checkCell, Color.red);
					Result = false;
					return Result;
				}
			}
		}

		return Result;
	}

	public static bool checkBuildingSize(Vector2 pos, Tilemap tileMap, int sizeX, int sizeY){
		bool Result = true;

		//Vector3Int clickedCell = tileMap.WorldToCell(pos);

		for(int iX = 0; iX < sizeX; iX++){
			Vector3 posTem = new Vector3(pos.x + (-iX * tileMap.cellSize.x),pos.y + (iX * tileMap.cellSize.y),0);
			for(int iY = 0; iY < sizeY; iY++){
				Vector3Int checkCell = tileMap.WorldToCell(new Vector3(posTem.x+(iY * tileMap.cellSize.x),posTem.y+(iY * tileMap.cellSize.y),0));
				if(!tileMap.HasTile(checkCell)){
					//tileMap.SetTileFlags(checkCell, TileFlags.None);
					//tileMap.SetColor(checkCell, Color.red);
					Result = false;
					return Result;
				}
			}
		}

		return Result;
	}

	public static bool checkHasRoad(Vector3 pos, Tilemap tileMap, bool flipped, int sizeY){
		bool Result = true;
		float flipMod = 1;
		if(flipped){
			flipMod = -1;
		}
		Vector3 posTemp = new Vector3(pos.x+(flipMod * tileMap.cellSize.x),pos.y - tileMap.cellSize.y,0);
		//Vector3Int clickedCell = tileMap.WorldToCell(pos);
			for(int iY = 0; iY < sizeY; iY++){
				Vector3Int checkCell = tileMap.WorldToCell(new Vector3(posTemp.x+(flipMod * iY * tileMap.cellSize.x),posTemp.y+(iY * tileMap.cellSize.y),0));
				if(!tileMap.HasTile(checkCell)){
					//tileMap.SetTileFlags(checkCell, TileFlags.None);
					//tileMap.SetColor(checkCell, Color.red);
					Result = false;
					return Result;
				}
			}

		return Result;
	}

	public static List<Vector3Int> getTilesByBuildingSize(Vector3 pos, Tilemap tileMap, int sizeX, int sizeY){
		List<Vector3Int> Result = new List<Vector3Int>();

		for(int iX = 0; iX < sizeX; iX++){
			Vector3 posTem = new Vector3(pos.x + (-iX * tileMap.cellSize.x),pos.y + (iX * tileMap.cellSize.y),0);
			for(int iY = 0; iY < sizeY; iY++){
				Vector3Int checkCell = tileMap.WorldToCell(new Vector3(posTem.x+(iY * tileMap.cellSize.x),posTem.y+(iY * tileMap.cellSize.y),0));
				if(tileMap.HasTile(checkCell)){
					Result.Add(checkCell);
				}
			}
		}

		return Result;
	}

	public static List<Vector3Int> getTilesByBuildingSize(Vector2 pos, Tilemap tileMap, int sizeX, int sizeY){
		List<Vector3Int> Result = new List<Vector3Int>();

		for(int iX = 0; iX < sizeX; iX++){
			Vector3 posTem = new Vector3(pos.x + (-iX * tileMap.cellSize.x),pos.y + (iX * tileMap.cellSize.y),0);
			for(int iY = 0; iY < sizeY; iY++){
				Vector3Int checkCell = tileMap.WorldToCell(new Vector3(posTem.x+(iY * tileMap.cellSize.x),posTem.y+(iY * tileMap.cellSize.y),0));
				if(tileMap.HasTile(checkCell)){
					Result.Add(checkCell);
				}
			}
		}

		return Result;
	}

	public static void RemoveTilesByBuildingSize(Vector3 pos, Tilemap tileMap, int sizeX, int sizeY){

		for(int iX = 0; iX < sizeX; iX++){
			Vector3 posTem = new Vector3(pos.x + (-iX * tileMap.cellSize.x),pos.y + (iX * tileMap.cellSize.y),0);
			for(int iY = 0; iY < sizeY; iY++){
				Vector3Int checkCell = tileMap.WorldToCell(new Vector3(posTem.x+(iY * tileMap.cellSize.x),posTem.y+(iY * tileMap.cellSize.y),0));
				if(tileMap.HasTile(checkCell)){
					tileMap.SetTile(checkCell,null);
				}
			}
		}
	}

	public static void RemoveTilesByBuildingSize(Vector2 pos, Tilemap tileMap, int sizeX, int sizeY){

		for(int iX = 0; iX < sizeX; iX++){
			Vector3 posTem = new Vector3(pos.x + (-iX * tileMap.cellSize.x),pos.y + (iX * tileMap.cellSize.y),0);
			for(int iY = 0; iY < sizeY; iY++){
				Vector3Int checkCell = tileMap.WorldToCell(new Vector3(posTem.x+(iY * tileMap.cellSize.x),posTem.y+(iY * tileMap.cellSize.y),0));
				if(tileMap.HasTile(checkCell)){
					tileMap.SetTile(checkCell,null);
				}
			}
		}
	}

	public static string getTilesAround(Vector3 pos, Tilemap[] tileMaps, Color color){
		string Result = "";

		Vector3Int checkBL = tileMaps[0].WorldToCell(new Vector3(pos.x-tileMaps[0].cellSize.x,pos.y-tileMaps[0].cellSize.y,0));
		Vector3Int checkBR = tileMaps[0].WorldToCell(new Vector3(pos.x+tileMaps[0].cellSize.x,pos.y-tileMaps[0].cellSize.y,0));
		Vector3Int checkTL = tileMaps[0].WorldToCell(new Vector3(pos.x-tileMaps[0].cellSize.x,pos.y+tileMaps[0].cellSize.y,0));
		Vector3Int checkTR = tileMaps[0].WorldToCell(new Vector3(pos.x+tileMaps[0].cellSize.x,pos.y+tileMaps[0].cellSize.y,0));

		Vector3Int[] orbitCells = {checkTL, checkTR, checkBR, checkBL};
		string[] orbitPositions = {"-TopLeft-","-TopRight-","-BotRight-","-BotLeft-"};

			for(int i=0; i<orbitCells.Length;i++){
				foreach(Tilemap t in tileMaps){
					if(t.HasTile(orbitCells[i])){
						if(!Result.Contains(orbitPositions[i])){
							Result += orbitPositions[i];
							Debug.DrawLine(tileMaps[0].GetCellCenterWorld(orbitCells[i]),pos,color);
						}
						
					}
				}
			}
			
		return Result;
	}

	public static string getTilesAround(Vector3Int pos, Tilemap[] tileMaps, Color color){
		string Result = "";

		Vector3Int checkBL = tileMaps[0].WorldToCell(new Vector3(pos.x-tileMaps[0].cellSize.x,pos.y-tileMaps[0].cellSize.y,0));
		Vector3Int checkBR = tileMaps[0].WorldToCell(new Vector3(pos.x+tileMaps[0].cellSize.x,pos.y-tileMaps[0].cellSize.y,0));
		Vector3Int checkTL = tileMaps[0].WorldToCell(new Vector3(pos.x-tileMaps[0].cellSize.x,pos.y+tileMaps[0].cellSize.y,0));
		Vector3Int checkTR = tileMaps[0].WorldToCell(new Vector3(pos.x+tileMaps[0].cellSize.x,pos.y+tileMaps[0].cellSize.y,0));

		Vector3Int[] orbitCells = {checkTL, checkTR, checkBR, checkBL};
		string[] orbitPositions = {"-TopLeft-","-TopRight-","-BotRight-","-BotLeft-"};

			for(int i=0; i<orbitCells.Length;i++){
				foreach(Tilemap t in tileMaps){
					if(t.HasTile(orbitCells[i])){
						if(!Result.Contains(orbitPositions[i])){
							Result += orbitPositions[i];
							Debug.DrawLine(tileMaps[0].GetCellCenterWorld(orbitCells[i]),pos,color);
						}
						
					}
				}
			}
			
		return Result;
	}

	public static string getTilesAround(Vector2 pos, Tilemap[] tileMaps, Color color){
		string Result = "";

		Vector3Int checkBL = tileMaps[0].WorldToCell(new Vector3(pos.x-tileMaps[0].cellSize.x,pos.y-tileMaps[0].cellSize.y,0));
		Vector3Int checkBR = tileMaps[0].WorldToCell(new Vector3(pos.x+tileMaps[0].cellSize.x,pos.y-tileMaps[0].cellSize.y,0));
		Vector3Int checkTL = tileMaps[0].WorldToCell(new Vector3(pos.x-tileMaps[0].cellSize.x,pos.y+tileMaps[0].cellSize.y,0));
		Vector3Int checkTR = tileMaps[0].WorldToCell(new Vector3(pos.x+tileMaps[0].cellSize.x,pos.y+tileMaps[0].cellSize.y,0));

		Vector3Int[] orbitCells = {checkTL, checkTR, checkBR, checkBL};
		string[] orbitPositions = {"-TopLeft-","-TopRight-","-BotRight-","-BotLeft-"};

			for(int i=0; i<orbitCells.Length;i++){
				foreach(Tilemap t in tileMaps){
					if(t.HasTile(orbitCells[i])){
						if(!Result.Contains(orbitPositions[i])){
							Result += orbitPositions[i];
							Debug.DrawLine(tileMaps[0].GetCellCenterWorld(orbitCells[i]),pos,color);
						}
						
					}
				}
			}
			
		return Result;
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

	

	public static Vector3Int[] getAllTilesOnAreaOnMap(Tilemap tileMap, Vector3Int originCell, int areaSizeH, int areaSizeV){
		List<Vector3Int> Result = new List<Vector3Int>();

		BoundsInt Area = new BoundsInt(originCell,new Vector3Int(areaSizeH,areaSizeV,1));
		Area.SetMinMax(new Vector3Int(originCell.x-areaSizeH,originCell.y-areaSizeV,-1),new Vector3Int(originCell.x+areaSizeH,originCell.y+areaSizeV,1));
		//BoundsInt Area = new BoundsInt(-areaSizeH,-areaSizeV,1,areaSizeH,areaSizeV,1);
		
		foreach(var pos in Area.allPositionsWithin){

			
			if(tileMap.HasTile(pos)){
				tileMap.SetTileFlags(pos, TileFlags.None);
				tileMap.SetColor(pos, Color.red);
				Debug.Log(pos.ToString());
				//Result.Add(pos);
			}
			
		}
		
		return Result.ToArray();
	}

}
