using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Info : MonoBehaviour {

	public List<Vector3Int> currentPath;

	public Vector3Int goalTest;

	public int Fear;

	public bool gotTreasure;

	public bool Detected;

	public float MoveSpeed;

	private void move(){
		bool tryMove = GoblinLeafAction.instance.MoveAlongPath(this);
		//print("Moveu? " + tryMove);
	}
	private void getpath(){
		currentPath = GoblinLeafAction.instance.GetPath(transform.position,goalTest,Fear);
	}
	
	void Start () {
		Vector3Int originCell = GridInfo.instance.Chao.WorldToCell(transform.position);
		
		Invoke("getpath",1);
		InvokeRepeating("move",3,0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
