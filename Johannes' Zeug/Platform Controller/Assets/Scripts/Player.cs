using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Controller2D))] 
public class Player : MonoBehaviour {

	// epi 3
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
<<<<<<< HEAD
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;

	public float moveSpeed = 6;
=======
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;

	float moveSpeed = 6;
>>>>>>> f7c6948ecd56f484a5fdd183bccc9c193fca32dd

	float gravity;

	float jumpVelocity;
<<<<<<< HEAD
	//Vector3 wallJumpVelocity = new Vector3 (6, 5, 0);    //second way of wj implementation, this one jumps by a fixed amount and neglects the entry angle
=======
>>>>>>> f7c6948ecd56f484a5fdd183bccc9c193fca32dd

	Vector3 velocity;   // vector describing the velocity per deltatime
	float velocityXSmoothing;

	Controller2D controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity =Mathf.Abs ( gravity) * timeToJumpApex;
<<<<<<< HEAD
		//print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);

		controller.collisions.wJReset ();
=======
		print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
>>>>>>> f7c6948ecd56f484a5fdd183bccc9c193fca32dd
	}
	
	void Update () 
	{
		//epi 3 , so we do not accumulate gravity
<<<<<<< HEAD
		if (controller.collisions.above || controller.collisions.below )
=======
		if (controller.collisions.above || controller.collisions.below)
>>>>>>> f7c6948ecd56f484a5fdd183bccc9c193fca32dd
		{
			velocity.y = 0;
		}

<<<<<<< HEAD


=======
>>>>>>> f7c6948ecd56f484a5fdd183bccc9c193fca32dd
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (Input.GetKeyDown (KeyCode.Space) && controller.collisions.below) {
			velocity.y = jumpVelocity;
<<<<<<< HEAD
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
=======
		}else 

		//walljump
		if (Input.GetKeyDown (KeyCode.Space) && (controller.collisions.wallLeft || controller.collisions.wallRight)) {
			velocity = 10*controller.collisions.velocityWj;
			//velocity.y = jumpVelocity;
>>>>>>> f7c6948ecd56f484a5fdd183bccc9c193fca32dd
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)? accelerationTimeGrounded: accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

}
