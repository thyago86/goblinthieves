using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class VisibilityArea : MonoBehaviour {

	public GameObject MaskPrefab;

	public Tilemap Chao;
	public Tilemap Obstacles;

	public int AreaSize;

	public int PoolSize;

	private Queue<GameObject> MaskPool = new Queue<GameObject>();

	private void MakePool(){
		for(int i=0;i<PoolSize;i++){
			GameObject mask = Instantiate(MaskPrefab);
			mask.transform.SetParent(transform);
			mask.SetActive(false);
			MaskPool.Enqueue(mask);
		}
	}

	public void MakeVisibleArea(Vector3Int currentPos){
		foreach (GameObject maskTile in MaskPool){
			maskTile.transform.position = currentPos;
		}

		Vector3Int[] allPos = GridHandler.getAllTilesVisibilityTilesAround(Chao,Obstacles,currentPos,AreaSize);
		foreach (Vector3Int pos in allPos){
			//print(pos.ToString());

			
			GameObject Maskinst = MaskPool.Dequeue();
			Maskinst.transform.position = Chao.GetCellCenterWorld(pos);
			Maskinst.SetActive(true);
			MaskPool.Enqueue(Maskinst);
			
		}
	}

	void Start () {
		MakePool();
	}
	
	void Update () {
	}

	void FixedUpdate(){
	}
}
