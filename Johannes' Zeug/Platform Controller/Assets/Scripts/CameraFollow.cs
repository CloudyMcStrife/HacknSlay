using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GameObject))] 
public class CameraFollow : MonoBehaviour {

	public GameObject player;

	Vector3 pos = new Vector3(0,0,-3);

	
	// Update is called once per frame
	void Update () {
		transform.position = player.transform.position+pos;
	}
}
