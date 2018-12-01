using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class VisibilityArea : MonoBehaviour {

	public GameObject MaskPrefab;

	public Tilemap Chao;
	public Tilemap Parede;
	public Tilemap Fog;

	public int AreaSize;

	private int PoolSize;

	private Queue<GameObject> MaskPool = new Queue<GameObject>();

	private void MakePool(){
		PoolSize = Mathf.CeilToInt(Mathf.Pow(AreaSize,2) + Mathf.Pow(AreaSize-1,2));

		for(int i=0;i<PoolSize;i++){
			GameObject mask = Instantiate(MaskPrefab);
			mask.transform.SetParent(transform);
			mask.SetActive(false);
			MaskPool.Enqueue(mask);
		}
	}

	public void ResetPool(){
		while(MaskPool.Count > 0){
			GameObject m = MaskPool.Dequeue();
			Destroy(m);
		}

		MakePool();
	}

	public void MakeVisibleArea(Vector3Int currentPos){
		foreach (GameObject maskTile in MaskPool){
			maskTile.SetActive(false);
		}

		Vector3Int[] allPos = GridHandler.getAllTilesVisibilityTilesAround(Chao,Parede,currentPos,AreaSize);

		foreach (Vector3Int pos in allPos){
			
			GameObject Maskinst = MaskPool.Dequeue();
			Maskinst.transform.position = Chao.GetCellCenterWorld(pos);
			Maskinst.SetActive(true);
			MaskPool.Enqueue(Maskinst);

			if(Fog.HasTile(pos)){
				Fog.SetTile(pos,null);
			}
			
		}
	}

	void Start () {
		MakePool();
		Chao = GridInfo.instance.Chao;
		Parede = GridInfo.instance.Parede;
		Fog = GridInfo.instance.Fog;
		MakeVisibleArea(Chao.WorldToCell(transform.position));
	}
	
	void Update () {
	}

	void FixedUpdate(){
	}
}
