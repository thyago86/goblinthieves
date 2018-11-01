using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_Base : MonoBehaviour {


	
	void Start () {
		
	}
	
	
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Goblins"){
			print(other.gameObject.name);
		}
	}
}
