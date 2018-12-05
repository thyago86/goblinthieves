using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TochaInteract : ObjInteract_Base {

	public bool Active;
	public GameObject Light;

	public override void Interaction(){
		Active = !Active;
		Light.SetActive(Active);
		if(Active == false){
			Light.GetComponent<VisibilityArea>().DisableVisibilityArea();
		}
	}

	void Start () {
		Light.SetActive(Active);
	}
	

	void Update () {
		
	}
}
