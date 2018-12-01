using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Jogador_Interact : MonoBehaviour {

	private Jogador_Info JogadorInfo;
	public float rayDist;
	public LayerMask layer;

	public void Interact(){
		Vector2 FacingDir = (JogadorInfo.PlayerMov.Chao.GetCellCenterWorld(JogadorInfo.FacingTile) - transform.position).normalized;
		RaycastHit2D hit = Physics2D.Raycast(transform.position,FacingDir,rayDist,layer);
		Debug.DrawRay(transform.position,FacingDir*rayDist,Color.blue,0.2f);

		if(hit){
			hit.collider.gameObject.SendMessage("Interaction");
			print(hit.collider.gameObject.name);
		}
	}
	
	void Start () {
		JogadorInfo = transform.GetComponent<Jogador_Info>();
		
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.D) && !JogadorInfo.PlayerMov.Moving){
			Interact();
		}
	}
}
