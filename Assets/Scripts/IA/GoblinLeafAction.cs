using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinLeafAction : SingletonMonoBehaviour<GoblinLeafAction> {

	public List<Vector3Int> GetPath(Vector3 posWorld, Vector3Int goal, int Fear){
		Vector3Int originCell = GridInfo.instance.Chao.WorldToCell(posWorld);
		List<Vector3Int> Result = GridHandler.AStar(GridInfo.instance.Chao,GridInfo.instance.Parede, originCell, goal, Fear);
		return Result;
	}

	public bool MoveAlongPath(Goblin_Info goblin){
		int currentIndex = goblin.currentPath.IndexOf(GridInfo.instance.Chao.WorldToCell(goblin.transform.position));

		if(currentIndex == -1){
			return true;
		}
		else if(currentIndex+1 >= goblin.currentPath.Count){
			if(GridInfo.instance.Doors.HasTile(GridInfo.instance.Chao.WorldToCell(goblin.transform.position))){
				MudarSala(goblin.transform);
			}
			return true;
		}
		else{
			StartCoroutine(Move(GridInfo.instance.Chao.GetCellCenterWorld(goblin.currentPath[currentIndex + 1]), goblin.transform, goblin.MoveSpeed));
			return false;
		}
	}

	public bool CheckForTreasure(){
		return true;
	}

	public Vector3Int GetHideSpot(){
		Vector3Int Result = new Vector3Int(0,0,0);
		return Result;
	}

	public Vector3Int GetDoor(bool repeat = false){
		Vector3Int Result = new Vector3Int(0,0,0);
		return Result;
	}

	public Vector3Int GetLitTorch(){
		Vector3Int Result = new Vector3Int(0,0,0);
		return Result;
	}

	public Vector3Int GetFallenAlly(){
		Vector3Int Result = new Vector3Int(0,0,0);
		return Result;
	}

	public Vector3Int GetNearestExit(){
		Vector3Int Result = new Vector3Int(0,0,0);
		return Result;
	}

	public bool SnuffTorch(){
		return true;
	}

	public bool SaveAlly(){
		return true;
	}

	public bool grabTreasure(){
		return true;
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
