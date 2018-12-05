using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class VisibilityArea : MonoBehaviour {

	public GameObject MaskPrefab;

	public Vector3Int[] PrevPos;

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
		
		if(PrevPos.Length > 0){
			foreach(Vector3Int p in PrevPos){
				if(GridInfo.instance.LitTiles.Contains(p)){
					GridInfo.instance.LitTiles.Remove(p);
				}
			}
		}
		

		Vector3Int[] allPos = GridHandler.getAllTilesVisibilityTilesAround(Chao,Parede,currentPos,AreaSize);
		PrevPos = allPos;
		foreach (Vector3Int pos in allPos){
			
			GameObject Maskinst = MaskPool.Dequeue();
			Maskinst.transform.position = Chao.GetCellCenterWorld(pos);
			Maskinst.SetActive(true);
			MaskPool.Enqueue(Maskinst);

			if(Fog.HasTile(pos)){
				Fog.SetTile(pos,null);
			}

			GridInfo.instance.LitTiles.Add(pos);
		}

		foreach(Vector3Int pos in GridInfo.instance.LitTiles){
			//Chao.SetTileFlags(pos, TileFlags.None);
			//Chao.SetColor(pos, Color.blue);
		}
	}

	public void DisableVisibilityArea(){
		foreach(Vector3Int p in PrevPos){
			if(GridInfo.instance.LitTiles.Contains(p)){
				GridInfo.instance.LitTiles.Remove(p);
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
