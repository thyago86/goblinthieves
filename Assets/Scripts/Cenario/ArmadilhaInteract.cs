using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilhaInteract : ObjInteract_Base {

	public bool Active;

	public GameObject touchedObj;
	public VisibilityArea Light;
	public Sprite ActiveSprite;

	public override void Interaction(){
		if(Active){
			touchedObj.SendMessage("Prender",SendMessageOptions.DontRequireReceiver);
		}else{
			touchedObj.SendMessage("PegarItem","Armadilha");
			Destroy(transform.gameObject);
		}
	}

	void Start () {
		if(Active){
			Light.gameObject.SetActive(true);
			gameObject.GetComponent<SpriteRenderer>().sprite = ActiveSprite;
		}else{
			Light.gameObject.SetActive(false);
		}
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.tag != "Ataque"){
			touchedObj = other.gameObject;
			Interaction();
		}
		
	}
}
