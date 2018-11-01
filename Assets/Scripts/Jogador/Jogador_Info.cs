using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Jogador_Info : MonoBehaviour {

	
	public Vector3Int FacingTile;
	public Jogador_Ataque PlayerAtk;
	public Jogador_Movimento PlayerMov;

	void Start () {
		PlayerAtk = transform.GetComponent<Jogador_Ataque>();
		PlayerMov = transform.GetComponent<Jogador_Movimento>();
	}
	
	
	void Update () {
		
	}
}
