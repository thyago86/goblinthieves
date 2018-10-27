using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Jogador_Movimento : MonoBehaviour {

/* Tilemaps */
	public Tilemap Chao;
	public Tilemap Obstaculos;
/* Tilemaps */

/* Referencias Externas */
	private Jogador_Info PlayerInfo;
/* Referencias Externas */

/* Variaveis */
	public float MoveSpeed;
	public bool Moving;
/* Variaveis */

/* Lerp de movimento do personagem (triggers de animacao coloca aqui tbm) */

	private IEnumerator Mover(Vector3 nextPos){
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
/* Lerp de movimento do personagem (triggers de animacao coloca aqui tbm) */

/* Atualizar Tile de interação */
	private void UpdateFacingTile(Vector3Int toUpdate){
			Chao.SetTileFlags(PlayerInfo.FacingTile, TileFlags.None);
			Chao.SetColor(PlayerInfo.FacingTile, Color.white);
			PlayerInfo.FacingTile = toUpdate;
			Chao.SetTileFlags(PlayerInfo.FacingTile, TileFlags.None);
			Chao.SetColor(PlayerInfo.FacingTile, Color.red);		
	}
/* Atualizar Tile de interação */

	void Start () {
		PlayerInfo = transform.GetComponent<Jogador_Info>();
	}
	
	void Update () {
		if(!Moving){
			Vector3 pos = transform.position;
			if(Input.GetKey("up")){

				Vector3Int destination = Chao.WorldToCell(new Vector3(pos.x,pos.y+Chao.cellSize.y,0));
				UpdateFacingTile(destination);
				
				if(Chao.HasTile(destination) && !Obstaculos.HasTile(destination)){
					UpdateFacingTile(Chao.WorldToCell(new Vector3(pos.x,pos.y+Chao.cellSize.y*2,0)));
					StopAllCoroutines();
					StartCoroutine(Mover(Chao.GetCellCenterWorld(destination)));
				}
			}else if(Input.GetKey("down")){

				Vector3Int destination = Chao.WorldToCell(new Vector3(pos.x,pos.y-Chao.cellSize.y,0));
				UpdateFacingTile(destination);
				
				if(Chao.HasTile(destination) && !Obstaculos.HasTile(destination)){
					UpdateFacingTile(Chao.WorldToCell(new Vector3(pos.x,pos.y-Chao.cellSize.y*2,0)));
					StopAllCoroutines();
					StartCoroutine(Mover(Chao.GetCellCenterWorld(destination)));	
				}
			}else if(Input.GetKey("left")){

				Vector3Int destination = Chao.WorldToCell(new Vector3(pos.x-Chao.cellSize.x,pos.y,0));
				UpdateFacingTile(destination);
				
				if(Chao.HasTile(destination) && !Obstaculos.HasTile(destination)){
					UpdateFacingTile(Chao.WorldToCell(new Vector3(pos.x-Chao.cellSize.x*2,pos.y,0)));
					StopAllCoroutines();
					StartCoroutine(Mover(Chao.GetCellCenterWorld(destination)));
				}
			}else if(Input.GetKey("right")){

				Vector3Int destination = Chao.WorldToCell(new Vector3(pos.x+Chao.cellSize.x,pos.y,0));
				UpdateFacingTile(destination);
				
				if(Chao.HasTile(destination) && !Obstaculos.HasTile(destination)){
					UpdateFacingTile(Chao.WorldToCell(new Vector3(pos.x+Chao.cellSize.x*2,pos.y,0)));
					StopAllCoroutines();
					StartCoroutine(Mover(Chao.GetCellCenterWorld(destination)));
				}
			}
		}
	}
}
