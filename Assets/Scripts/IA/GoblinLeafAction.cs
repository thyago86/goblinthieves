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

	public Vector3Int GetNearestLitTorch(Vector3Int originCell){
		Vector3Int Result = new Vector3Int(0,0,0);

		List<GameObject> AllInteractables = GridHandler.GetInteractablesOnRoom(GridInfo.instance.currentmapBounds);

		float checkDist = 100000;
		foreach(GameObject interact in AllInteractables){
			TochaInteract torchInfo = interact.GetComponent<TochaInteract>();
			Vector3Int interactPos = GridInfo.instance.Chao.WorldToCell(interact.transform.position);
			if(torchInfo && torchInfo.Active){
				if(Vector3Int.Distance(originCell,interactPos) < checkDist){
					Result = interactPos;
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

	public Vector3Int GetNearestFallenAlly(Vector3Int originCell){
		Vector3Int Result = new Vector3Int(0,0,0);

		List<GameObject> AllInteractables = GridHandler.GetGoblinsOnRoom(GridInfo.instance.currentmapBounds);

		float checkDist = 100000;
		foreach(GameObject interact in AllInteractables){
			Goblin_Info allyInfo = interact.GetComponent<Goblin_Info>();
			Vector3Int interactPos = GridInfo.instance.Chao.WorldToCell(interact.transform.position);
			if(allyInfo && allyInfo.Defeated){
				if(Vector3Int.Distance(originCell,interactPos) < checkDist){
					Result = interactPos;
					checkDist = Vector3Int.Distance(originCell,interactPos);
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

	public bool SnuffTorch(TochaInteract tocha){
		tocha.Interaction();
		return true;
	}

	public bool SaveAlly(Goblin_Info ally){
		ally.Defeated = false;
		return true;
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
