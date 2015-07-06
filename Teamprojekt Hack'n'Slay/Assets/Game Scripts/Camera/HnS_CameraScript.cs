using UnityEngine;
using System.Collections;


public class HnS_CameraScript : MonoBehaviour {
	
	public HnS_MainCharacter target;
	private float goalZDistance;
	private RaycastHit rayHit;
	private Ray downRay;
	private const float standardDistance = -5;
	private float currentAccelleration = 0;
	//bool rayCastCollision = false;

	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		/*downRay = new Ray (target.transform.position, -Vector3.up);

		print ("RayCast collided: " + rayCastCollision);

		if (!target.m_IsGrounded) {
			rayCastCollision = Physics.Raycast (downRay, out rayHit);
			if (rayCastCollision) {
				goalZDistance = standardDistance - rayHit.distance;
			}
		} else {
			//goalZDistance = standardDistance;
		}
		*/

		goalZDistance = standardDistance - target.m_totalGroundDistance;

		float diff = goalZDistance - this.transform.position.z;
		//currentAccelleration = Mathf.Sign(diff)*diff*diff;
		//currentAccelleration = Mathf.Sign (diff) * diff * diff;
		this.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z + 5*diff*Time.deltaTime); 
	}
}
