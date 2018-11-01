using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Jogador_Ataque : MonoBehaviour {

	public GameObject AtkWeaponPrefab;
	public GameObject AtkWeaponInst;

	private Jogador_Info PlayerInfo;

	private void Attack(){
		if(!PlayerInfo.PlayerMov.Obstaculos.HasTile(PlayerInfo.FacingTile)){
			Vector2 FacingDir = (PlayerInfo.PlayerMov.Chao.GetCellCenterWorld(PlayerInfo.FacingTile) - transform.position).normalized;
			float rotZ = Mathf.Atan2(FacingDir.y,FacingDir.x) * Mathf.Rad2Deg;

			AtkWeaponInst = Instantiate(AtkWeaponPrefab,PlayerInfo.PlayerMov.Chao.GetCellCenterWorld(PlayerInfo.FacingTile),Quaternion.Euler(new Vector3(0,0,rotZ)));
			//transform.position
			Destroy(AtkWeaponInst,0.3f);
		}
	}

	void Start () {
		PlayerInfo = transform.GetComponent<Jogador_Info>();
	}
	

	void Update () {
		if(Input.GetKeyDown("space") && !AtkWeaponInst && !PlayerInfo.PlayerMov.Moving){
			Attack();
		}
	}
}
