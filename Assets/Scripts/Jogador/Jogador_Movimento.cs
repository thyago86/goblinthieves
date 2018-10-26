using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Jogador_Movimento : MonoBehaviour {

	public Tilemap Chao;
	public float MoveSpeed;
	public bool Moving;

	private IEnumerator Mover(Vector3 nextPos){
		print(nextPos.ToString());
		Moving = true;
		while(Moving){
			if(Vector3.Distance(transform.position, nextPos) > 0){
				transform.position = Vector3.MoveTowards(transform.position, nextPos, MoveSpeed*Time.deltaTime);
			}else{
				Moving = false;
			}
			yield return null;
		}
		yield return null;
	}


	void Start () {
		
	}
	
	
	void Update () {
		if((Input.GetKeyDown("up") || Input.GetKeyDown("down") || Input.GetKeyDown("left") || Input.GetKeyDown("right")) && !Moving){
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			print(h);
			print(v);
			Vector3 pos = transform.position;
			Vector3Int destination = Chao.WorldToCell(new Vector3(pos.x+Chao.cellSize.x * h,pos.y+Chao.cellSize.y * v,0));
			print(pos.ToString());
			print(destination.ToString());
			
			Mover(Chao.GetCellCenterWorld(destination));
		}
	}
}
