using UnityEngine;
using System.Collections;

public class aiFollower : MonoBehaviour {

	public Transform target;
	public float speedRotation = 2;
	public float movingSpeed = 2;
	public float minimalDistance = 2;
	public float fromSurfaceToObject=1;
	public float surfaceSpeedDown = 100;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



			RotateTo (target.position);
			Walk (target.position);
			SurfaceDetection();

	
	}

	void RotateTo(Vector3 targetPos){


		Quaternion destRotation;
		Vector3 relativePos;

		relativePos = targetPos - transform.position;
		relativePos.y = 0;
		destRotation= Quaternion.LookRotation (relativePos);



		transform.rotation = Quaternion.Slerp(transform.rotation, destRotation, speedRotation*Time.deltaTime);


	}

	void Walk(Vector3 targetPosition){

	
		Vector3 richtung = transform.TransformDirection (Vector3.forward);

		Vector3 delta = targetPosition - transform.position;
		Rigidbody r = (Rigidbody)GetComponent (typeof(Rigidbody));
	
			if (delta.magnitude > minimalDistance) { //magnitude gibt Länge des Vektors
			
				r.velocity = richtung * movingSpeed;

			} else {
				r.velocity = Vector3.zero;

			}
				//setzt die Geschw. dann auf 0

	}





	void SurfaceDetection(){
		RaycastHit hit;
		Rigidbody rigidbody = (Rigidbody)GetComponent (typeof(Rigidbody));
		if (Physics.Raycast (transform.position, -Vector3.up, out hit,  //gibt boolean zurück, ob was berührt wurde
		                fromSurfaceToObject)) 
		{

		} else
		{
			rigidbody.velocity = rigidbody.velocity + (-Vector3.up * surfaceSpeedDown * Time.deltaTime);
		}
	Debug.DrawRay(transform.position, -Vector3.up * fromSurfaceToObject);
	}
}

