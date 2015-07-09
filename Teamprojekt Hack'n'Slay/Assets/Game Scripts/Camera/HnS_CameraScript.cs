using UnityEngine;
using System.Collections;


public class HnS_CameraScript : MonoBehaviour {
	
	public HnS_MainCharacter target;
	private float goalZDistance;
	private RaycastHit rayHit;
	private Ray downRay;
	private const float standardDistance = -5;
	//private float targetHeightHalfed;
	//bool rayCastCollision = false;

	
	
	// Use this for initialization
	void Start () {
		//targetHeightHalfed = target.m_height / 2;

		//print ("CameraScript m_height: " + targetHeightHalfed);
	}
	
	// Update is called once per frame
	void Update () {

		goalZDistance = standardDistance - target.m_totalGroundDistance;

		float diff = goalZDistance - this.transform.position.z;
		//currentAccelleration = Mathf.Sign(diff)*diff*diff;
		//currentAccelleration = Mathf.Sign (diff) * diff * diff;


		// The '2' in the y-dimension is for compensation of the MainCharacter's height. Its local center is set down at the feet, so that's where the camera WOULD look
		this.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2, this.transform.position.z + 5*diff*Time.deltaTime); 
	}
}
