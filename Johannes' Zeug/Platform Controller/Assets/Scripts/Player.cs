using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Controller2D))] 
public class Player : MonoBehaviour {

	// epi 3
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;

	public float moveSpeed = 6;

	float gravity;

	float jumpVelocity;
	//Vector3 wallJumpVelocity = new Vector3 (6, 5, 0);    //second way of wj implementation, this one jumps by a fixed amount and neglects the entry angle

	Vector3 velocity;   // vector describing the velocity per deltatime
	float velocityXSmoothing;

	Controller2D controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity =Mathf.Abs ( gravity) * timeToJumpApex;
		//print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);

		controller.collisions.wJReset ();
	}
	
	void Update () 
	{
		//epi 3 , so we do not accumulate gravity
		if (controller.collisions.above || controller.collisions.below )
		{
			velocity.y = 0;
		}



		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (Input.GetKeyDown (KeyCode.Space) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		} else 

			//print (" walLeft:" + controller.collisions.wallLeft + "wallRight:" + controller.collisions.wallRight);
		//walljump
		if (Input.GetKeyDown (KeyCode.Space) && (controller.collisions.wallLeft || controller.collisions.wallRight)&& !controller.collisions.below) {
			velocity = Vector3.zero;

			//print ("velX:" + velocity.x + "velY:" + velocity.y);

			velocity = controller.collisions.velocityWj;    // entry and exit angle govern jump direction



			print ("velX:" + velocity.x + "velY:" + velocity.y);
			//velocity.y = jumpVelocity;

			controller.collisions.wJReset();
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)? accelerationTimeGrounded: accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

}
