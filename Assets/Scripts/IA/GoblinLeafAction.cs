using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinLeafAction : MonoBehaviour {

	private Goblin_Info goblin;
	
	private void Start(){
		goblin = transform.GetComponent<Goblin_Info>();
	}

	public List<Vector3Int> GetPath(Vector3 posWorld, Vector3Int goal, int Fear){
		Vector3Int originCell = GridInfo.instance.Chao.WorldToCell(posWorld);
		List<Vector3Int> Result = GridHandler.AStar(GridInfo.instance.Chao,GridInfo.instance.Parede, originCell, goal, Fear);
		return Result;
	}

	public void MoveAlongPath(Goblin_Info goblin){
		int currentIndex = goblin.currentPath.IndexOf(GridInfo.instance.Chao.WorldToCell(goblin.transform.position));
		if(currentIndex < goblin.currentPath.Count-1){
			StopAllCoroutines();
			StartCoroutine(Move(GridInfo.instance.Chao.GetCellCenterWorld(goblin.currentPath[currentIndex + 1]), goblin.transform, goblin.MoveSpeed));
		}
	}

	public bool CanMoveAlongPath(Goblin_Info goblin){
		int currentIndex = goblin.currentPath.IndexOf(GridInfo.instance.Chao.WorldToCell(goblin.transform.position));

		if(currentIndex == -1 || currentIndex >= goblin.currentPath.Count){
			return false;
		}
		else{
			return true;
		}
	}

	public bool CheckForTreasure(BoundsInt roomBounds){
		GameObject Treasure = GameObject.FindGameObjectWithTag("Treasure");
		if(!goblin.VisitedSectors.Contains(roomBounds)){
			goblin.VisitedSectors.Add(roomBounds);
		}
		if(Treasure && roomBounds.Contains(GridInfo.instance.Chao.WorldToCell(Treasure.transform.position))){
			return true;
		}else{
			return false;
		}
	}

	public bool CheckForAlly(BoundsInt roomBounds){
		GameObject ally = GameObject.FindGameObjectWithTag("Goblins");
		if(ally && roomBounds.Contains(GridInfo.instance.Chao.WorldToCell(ally.transform.position))){
			return true;
		}else{
			return false;
		}
	}

	public bool CheckForHideSpot(Vector3Int originCell){
		if(GridInfo.instance.HideSpots.HasTile(originCell) && originCell == goblin.goal){
			return true;
		}else{
			return false;
		}
	}

	public Vector3Int GetHideSpot(Vector3Int originCell, BoundsInt roomBounds){
		Vector3Int Result = new Vector3Int(0,0,0);

		List<Vector3Int> allHideSpots = GridHandler.getAllTilesOnMap(GridInfo.instance.HideSpots);
		Jogador_Info player = FindObjectOfType<Jogador_Info>();
		Vector3Int playerPos = GridInfo.instance.Chao.WorldToCell(player.transform.position);
		float checkDist = 0;
		foreach(Vector3Int pos in allHideSpots){
			
			if(!GridInfo.instance.LitTiles.Contains(pos) && roomBounds.Contains(pos)){
				if(Vector3Int.Distance(playerPos,pos) > checkDist){
					Result = pos;
					checkDist = Vector3Int.Distance(playerPos,pos);
				}
			}
			
		}
		

		print(Result);
		return Result;
	}

	public BoundsInt GetNextRandomSector(Vector3Int originCell){
		BoundsInt nextSector = new BoundsInt(new Vector3Int(0,0,0),Vector3Int.zero);
		while(nextSector.size == Vector3Int.zero){
			int random = Random.Range(0, GridInfo.instance.mapBounds.Count);
			if(!goblin.VisitedSectors.Contains(GridInfo.instance.mapBounds[random]) && goblin.currentRoom != GridInfo.instance.mapBounds[random]){
				nextSector = GridInfo.instance.mapBounds[random];
			}	
		}
		return nextSector;
	}

	public bool CheckForPlayer(){
		Jogador_Info player = FindObjectOfType<Jogador_Info>();
		Vector3Int currentPos = GridInfo.instance.Chao.WorldToCell(transform.position);
		//print(currentPos);

		Vector3Int[] sightArea = player.PlayerMov.Sight.PrevPos;
		
		foreach(Vector3Int pos in sightArea){
			//print(pos.ToString());
			if(pos == currentPos){
				return true;
				
			}
		}
		return  false;
	}

	public TochaInteract GetNearestLitTorch(Vector3Int originCell){
		TochaInteract Result = null;

		List<GameObject> AllInteractables = GridHandler.GetInteractablesOnRoom(goblin.currentRoom);

		float checkDist = 100000;
		foreach(GameObject interact in AllInteractables){
			print(interact.name);
			TochaInteract torchInfo = interact.GetComponent<TochaInteract>();
			Vector3Int interactPos = GridInfo.instance.Chao.WorldToCell(interact.transform.position);
			if(torchInfo && torchInfo.Active){
				if(Vector3Int.Distance(originCell,interactPos) < checkDist){
					Result = torchInfo;
					checkDist = Vector3Int.Distance(originCell,interactPos);
				}
			}
		}
		
		return Result;
	}

	public Vector3Int GetNearestTreasure(Vector3Int originCell){
		Vector3Int Result = new Vector3Int(0,0,0);

		GameObject[] Treasures = GameObject.FindGameObjectsWithTag("Treasure");

		float checkDist = 100000;
		foreach(GameObject t in Treasures){
			Vector3Int treasuretPos = GridInfo.instance.Chao.WorldToCell(t.transform.position);
			if(Vector3Int.Distance(originCell,treasuretPos) < checkDist){
				Result = treasuretPos;
				checkDist = Vector3Int.Distance(originCell,treasuretPos);
			}
		}
		
		return Result;
	}

	public Goblin_Info GetNearestFallenAlly(Vector3Int originCell){
		Goblin_Info Result = null;

		List<GameObject> AllInteractables = GridHandler.GetGoblinsOnRoom(goblin.currentRoom);

		float checkDist = 100000;
		foreach(GameObject ally in AllInteractables){
			Goblin_Info allyInfo = ally.GetComponent<Goblin_Info>();
			Vector3Int allyPos = GridInfo.instance.Chao.WorldToCell(ally.transform.position);
			if(allyInfo && allyInfo.Defeated && goblin.currentRoom.Contains(allyPos)){
				if(Vector3Int.Distance(originCell,allyPos) < checkDist){
					Result = allyInfo;
					checkDist = Vector3Int.Distance(originCell,allyPos);
				}
			}
		}
		
		return Result;
	}

	public Vector3Int GetNearestExit(Vector3Int originCell){
		Vector3Int Result = new Vector3Int(0,0,0);

		List<Vector3Int> allExits = GridHandler.getAllTilesOnMap(GridInfo.instance.Exits);
		float checkDist = 100000;
		foreach( Vector3Int pos in allExits){
			if(Vector3Int.Distance(originCell,pos) < checkDist){
				Result = pos;
				checkDist = Vector3Int.Distance(originCell,pos);
			}
		}
		
		return Result;
	}

	public void SnuffTorch(){
		GameObject[] AllTorches = GameObject.FindGameObjectsWithTag("Interactable");

		foreach(GameObject t in AllTorches){
			TochaInteract tInfo = t.GetComponent<TochaInteract>();
			if(Vector3.Distance(transform.position, t.transform.position) <= 0.2 && tInfo.Active){
				tInfo.Active = false;
			}
		}
	}

	public void SaveAlly(){
		GameObject[] AllAllies = GameObject.FindGameObjectsWithTag("Goblins");

		foreach(GameObject Gob in AllAllies){
			if(Vector3.Distance(transform.position, Gob.transform.position) <= 0.2 && Gob.GetComponent<Goblin_Info>().Defeated){
				Gob.GetComponent<Goblin_Info>().Defeated = false;
			}
		}
	}

	public void grabTreasure(){
		GameObject[] AllTreasures = GameObject.FindGameObjectsWithTag("Treasure");

		foreach(GameObject Treasure in AllTreasures){
			if(Vector3.Distance(transform.position, Treasure.transform.position) <= 0.2){
				goblin.gotTreasure = true;
				Destroy(Treasure);
			}
		}
		
	}

	private IEnumerator Move(Vector3 targetPos, Transform actor, float MoveSpeed){

		bool Moving = true;
		
		while(Moving){
			if(Vector3.Distance(actor.position, targetPos) > 0){
				actor.position = Vector3.MoveTowards(actor.position, targetPos, MoveSpeed*Time.deltaTime);
			}else{
				Moving = false;
			}
			yield return null;
		}
		yield return null;
	}

	public void MudarSala(Transform actor){
		Vector3Int PosFinal = PortalHandler.instance.Portal_Portal[GridInfo.instance.Chao.WorldToCell(actor.position)];
		actor.position = GridInfo.instance.Chao.GetCellCenterWorld(PosFinal);
	}

}
