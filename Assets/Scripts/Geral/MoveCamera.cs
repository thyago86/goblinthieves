using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

	public int MoveSpeed;

	public Vector3[] CameraSpots;

	public void CallMoveCamera(Vector3 Spot){
		StopAllCoroutines();
		StartCoroutine(Mover(Spot));
	}

	private IEnumerator Mover(Vector3 nextPos){
		while(Vector3.Distance(transform.position, nextPos) > 0){
			transform.position = Vector3.MoveTowards(transform.position, nextPos, MoveSpeed*Time.deltaTime);
			yield return null;
		}
	}


	void Start () {
		
	}
	
	
	void Update () {
		
	}
}
