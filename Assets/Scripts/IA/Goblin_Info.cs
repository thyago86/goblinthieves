using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Goblin_Info : MonoBehaviour {

	public List<Vector3Int> currentPath;

	public List<BoundsInt> VisitedSectors = new List<BoundsInt>();

	public Vector3Int goal;

	public BoundsInt currentRoom;

	public BoundsInt targetRoom;

	public Vector3Int originCell;

	private GoblinLeafAction Actions;

	public int Fear;

	public int FearDefault;

	public bool gotTreasure;

	public bool Safe;

	public bool Detected;

	public bool Defeated;

	public float MoveSpeed;


	[Task]
	private void move(){
		Actions.MoveAlongPath(this);
		Task.current.Succeed();
	}

	[Task]
	private void canMove(){
		bool tryMove = Actions.CanMoveAlongPath(this);
		Task.current.Complete(tryMove);
		
	}

	[Task]
	private void getHidepath(){
		if(goal != Actions.GetHideSpot(originCell, currentRoom)){
			goal = Actions.GetHideSpot(originCell, currentRoom);
			currentPath = Actions.GetPath(transform.position,goal,Fear);
		}
		Task.current.Succeed();
	}

	[Task]
	private void getTreasurepath(){
		if(goal != Actions.GetNearestTreasure(originCell)){
			goal = Actions.GetNearestTreasure(originCell);
			currentPath = Actions.GetPath(transform.position,goal,Fear);
		}
		
		Task.current.Succeed();
	}
 
	[Task]
	private void getExitpath(){
		if(goal != Actions.GetNearestExit(originCell)){
			goal = Actions.GetNearestExit(originCell);
			currentPath = Actions.GetPath(transform.position,goal,Fear);
		}
		
		Task.current.Succeed();
	}

	[Task]
	private void getDeadAllypath(){
		Goblin_Info Ally = Actions.GetNearestFallenAlly(originCell);
		Vector3Int AllyPos = GridInfo.instance.Chao.WorldToCell(Ally.transform.position);
		if(goal != AllyPos){
			goal = AllyPos;
			currentPath = Actions.GetPath(transform.position,goal,Fear);
		}
		
		Task.current.Succeed();
	}

	[Task]
	private void getTorchpath(){
		TochaInteract tocha = Actions.GetNearestLitTorch(originCell);
		Vector3Int tochaPos = GridInfo.instance.Chao.WorldToCell(tocha.transform.position);
		if(goal != tochaPos){
			goal = tochaPos;
			currentPath = Actions.GetPath(transform.position,goal,Fear);
		}
		
		Task.current.Succeed();
	}

	[Task]
	private void getNextSector(){
		BoundsInt nextS = new BoundsInt();
		if(GridHandler.GetRoomBounds(originCell) != currentRoom || goal == originCell){
			currentRoom = GridHandler.GetRoomBounds(originCell);
			nextS = Actions.GetNextRandomSector(originCell);
			targetRoom = nextS;
		}else{
			nextS = targetRoom;
		}
		
		
		
		if(goal.x != nextS.center.x && goal.y != nextS.center.y){
			goal = GridInfo.instance.Chao.WorldToCell(nextS.center);
			currentPath = Actions.GetPath(transform.position,goal,Fear);
		}
		
		Task.current.Succeed();
	}

 
	[Task]
	private void grabTreasure(){
		Actions.grabTreasure();
		Task.current.Succeed();
	}

	[Task]
	private void ReviveAlly(){
		Actions.SaveAlly();
		Task.current.Succeed();
	}

	[Task]
	private void trySnuffTorch(){
		Actions.SnuffTorch();
		Task.current.Succeed();
	}


	[Task]
	private void IgotTreasure(){
		Task.current.Complete(gotTreasure);
	}

	[Task]
	private void IsDead(){
		Task.current.Complete(Defeated);
	}

	[Task]
	private void AllyDeadInRoom(){
		Goblin_Info Ally = Actions.GetNearestFallenAlly(originCell);
		if(Ally != null){
			Task.current.Succeed();
		}else{
			Task.current.Fail();
		}
		
	}

	[Task]
	private void TorchInRoom(){
		TochaInteract tocha = Actions.GetNearestLitTorch(originCell);
		print(tocha);
		if(tocha != null){
			Task.current.Succeed();
		}else{
			Task.current.Fail();
		}
		
	}
	
	[Task]
	private void onSight(){
		Task.current.Complete(Detected);
	}

	[Task]
	private void isSafe(){
		Task.current.Complete(Safe);
	}

	[Task]
	private void RoomTreasure(){
		BoundsInt curretBounds = GridHandler.GetRoomBounds(originCell);
		Task.current.Complete(Actions.CheckForTreasure(curretBounds));
	}

	[Task]
	private void tryBecomeSafe(){
		Safe = Actions.CheckForHideSpot(originCell);
		if(Safe){
			Fear = FearDefault;
		}else{
			Fear = 100;
		}
		Task.current.Succeed();
	}
	
	void Start () {
		Actions = transform.GetComponent<GoblinLeafAction>();
		
		//Invoke("getExitpath",1);
		//InvokeRepeating("move",3,0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		originCell = GridInfo.instance.Chao.WorldToCell(transform.position);
		Detected = Actions.CheckForPlayer();
		if(Detected){
			Safe = false;
		}
	}
}
