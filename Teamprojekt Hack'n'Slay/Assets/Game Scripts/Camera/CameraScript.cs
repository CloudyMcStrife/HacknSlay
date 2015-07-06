using UnityEngine;
using System.Collections;


public class CameraScript : MonoBehaviour {

	public GameObject target;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		this.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, this.gameObject.transform.position.z); 
	}
}
