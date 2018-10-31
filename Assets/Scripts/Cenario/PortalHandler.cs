using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHandler : MonoBehaviour {

	public static Dictionary<Vector3Int,Vector3Int> Portal_Portal = new Dictionary<Vector3Int,Vector3Int>();
	public static Dictionary<Vector3Int,Vector3> Portal_CameraSpot = new Dictionary<Vector3Int,Vector3>();

	void Start () {

		/* Teste - Vai ser substituido qdo tiver a geracao procedural */
			Portal_Portal.Add(new Vector3Int(-12,1,0),new Vector3Int(-15,1,0));
			Portal_Portal.Add(new Vector3Int(-15,1,0),new Vector3Int(-12,1,0));

			Portal_Portal.Add(new Vector3Int(-6,-7,0),new Vector3Int(-6,-10,0));
			Portal_Portal.Add(new Vector3Int(-6,-10,0),new Vector3Int(-6,-7,0));

			Portal_Portal.Add(new Vector3Int(1,6,0),new Vector3Int(1,9,0));
			Portal_Portal.Add(new Vector3Int(1,9,0),new Vector3Int(1,6,0));

			Portal_Portal.Add(new Vector3Int(11,-1,0),new Vector3Int(14,-1,0));
			Portal_Portal.Add(new Vector3Int(14,-1,0),new Vector3Int(11,-1,0));

			Portal_CameraSpot.Add(new Vector3Int(-12,1,0),new Vector3(0,0,-1));
			Portal_CameraSpot.Add(new Vector3Int(11,-1,0),new Vector3(0,0,-1));
			Portal_CameraSpot.Add(new Vector3Int(1,6,0),new Vector3(0,0,-1));
			Portal_CameraSpot.Add(new Vector3Int(-6,-7,0),new Vector3(0,0,-1));

			Portal_CameraSpot.Add(new Vector3Int(-15,1,0),new Vector3(-26,0,-1));
			Portal_CameraSpot.Add(new Vector3Int(-6,-10,0),new Vector3Int(0,-16,-1));
			Portal_CameraSpot.Add(new Vector3Int(1,9,0),new Vector3Int(0,16,-1));
			Portal_CameraSpot.Add(new Vector3Int(14,-1,0),new Vector3Int(26,0,-1));
		/* Teste - Vai ser substituido qdo tiver a geracao procedural */

	}
	
	
	void Update () {
		
	}
}
