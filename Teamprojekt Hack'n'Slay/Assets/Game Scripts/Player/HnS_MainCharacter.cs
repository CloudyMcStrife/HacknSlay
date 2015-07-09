using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
public class HnS_MainCharacter : MonoBehaviour 
{
	Rigidbody m_Rigidbody;
	Animator m_Animator;
	Collider m_Capsule;

	private float m_goalVelocity;
	private const float m_horizontalMaxSpeed = 15.0f;
	private float m_currentAccelleration = 0;
	private float m_currentVelocity = 0;
	private const float m_jumpPower = 10.0f;
	//private bool m_jumpRequest = false;
	//private float m_GroundCheckDistance = 0.5f;
	private bool m_isFacingRight;
	private Ray m_downRay; 
	private const float aerialMovDamping = 0.1f;
	private bool doubleJumpReady = true; 
	

	public RaycastHit m_downRayHit;
	public float m_totalGroundDistance = 0;
	public bool m_IsGrounded;
	public float m_height;


	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<SphereCollider>();
		m_isFacingRight = true;


		m_height = this.transform.localScale.y * this.GetComponent<SphereCollider>().radius * 2;

		print ("MainCharacter m_height:" + m_height);

		//m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation = true;
		
	}
	
	// Update is called once per frame
	void Update () {

		// the bigger the difference in currentVelocity and goalVelocity, the stronger the accelleration will be. If equal, accelleration should be 0
		float diff = m_goalVelocity - m_Rigidbody.velocity.x;

		// Accelleration/Controls depends on ground and airborne-state
		m_currentAccelleration = (m_IsGrounded) ? Mathf.Sign(diff)*diff*diff : Mathf.Sign(diff)*diff*diff*aerialMovDamping;
		//m_currentVelocity = m_Rigidbody.velocity.x +  5*m_currentAccelleration* Time.deltaTime;
		m_currentVelocity = m_Rigidbody.velocity.x +  m_currentAccelleration * Time.deltaTime;



		//print ("Sign: " + Mathf.Sign (goalVelocity - this.m_Rigidbody.velocity.x));
		//print ("diff: " + diff);
		//print ("currentAcceleration: " + m_currentAccelleration);
		//print ("goalVelocity: " + m_goalVelocity);
		//print ("currentVelocity: " + m_currentVelocity);
		//print ("RigidBody.velocity.x: " + m_Rigidbody.velocity.x);
		//print ("currentVelocity: " + m_currentVelocity);
		//print ("isGrounded: " + m_IsGrounded);


		//m_Rigidbody.velocity = new Vector3 (m_currentVelocity, m_Rigidbody.velocity.y, 0);

		//this.transform.position = new Vector3 (this.transform.position.x + Time.deltaTime * m_goalVelocity, this.transform.position.y, this.transform.position.z);

		m_Animator.SetFloat ("WalkSpeed", m_goalVelocity);
		//print ("goalVelocity: " + m_goalVelocity);


		/*
		if (jumpRequest) {
			m_Rigidbody.velocity = new Vector3 (m_velocity.x, m_velocity.y + jumpPower, 0);
			jumpRequest = false;
			/*
			jumpTimer += Time.deltaTime;
			if(jumpTimer > 1.0f){
				jumpTimer = 0;
				jumpRequest = false;
			}

		} else {
			m_Rigidbody.velocity = new Vector3 (m_velocity.x, m_velocity.y, 0);
		}
		*/


		/*
		this.gameObject.transform.position = new Vector3 (
				this.gameObject.transform.position.x + currentVelocity * Time.deltaTime, 
				this.gameObject.transform.position.y, 
				this.gameObject.transform.position.z
			);
	*/

		/*
		// Move the object forward along its z axis 1 unit/second.
		sumTime += Time.deltaTime;
		if (sumTime > 3) 
		{
			sumTime = 0;
			flip ();
		}

		if (m_isFacingRight) {
			transform.Translate(Vector3.right * Time.deltaTime, Space.World);
		} else {
			transform.Translate(Vector3.left * Time.deltaTime, Space.World);
		}
		*/
	}

	// h beschreibt Anteil an maximaler x-Geschwindigkeit in Intervall [-1, 1]
	public void move(float h, float v, bool jump)
	{

		CheckGroundStatus ();



		// check is input-direction is different to direction the character is facing
		// but only if any movement is called (h!=0)
		if ((m_isFacingRight != (h > 0)) && (h != 0)) 
		{

			// flip the player
			flip();
		}


		if (m_IsGrounded)
		{
			HandleGroundedMovement(h, jump);
		}
		else
		{
			HandleAirborneMovement(h, v, jump);
		}

	
		m_goalVelocity = h * m_horizontalMaxSpeed;

		//updateAnimator (h, v);
	}

	void HandleGroundedMovement (float h, bool jump)
	{
		if (jump) {

			print ("jump");
			//m_jumpRequest = true;
			//m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_jumpPower, m_Rigidbody.velocity.z);
			this.transform.position = this.transform.position + new Vector3(0, m_jumpPower/5, 0);
		}
	}
	
	void HandleAirborneMovement (float h, float v, bool jump)
	{
		if (doubleJumpReady && jump) {
			print ("doubleJump BITCHES!");
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_jumpPower, m_Rigidbody.velocity.z);
			doubleJumpReady = false;
		}
	}

	void CheckGroundStatus()
	{
		//RaycastHit hitInfo;
		#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + new Vector3(0, m_height/2, 0), transform.position + new Vector3(0, m_height/2, 0) + (Vector3.down * m_height/2), Color.red, 1);
		#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		//if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		m_downRay = new Ray (this.transform.position, -Vector3.up);
		Physics.Raycast (m_downRay, out m_downRayHit);

		m_totalGroundDistance = m_downRayHit.distance; // - m_GroundCheckDistance;


		if(m_totalGroundDistance > 0.1)
		{
			if(m_IsGrounded == true)
			{
				print ("ascension");
			}
				
			m_IsGrounded = false;
			m_Animator.SetBool("Grounded", false);
		}
		else
		{
			if(m_IsGrounded == false)
			{
				print ("landed - doublejump ready");
				doubleJumpReady = true;
			}

			m_IsGrounded = true;
			m_Animator.SetBool("Grounded", true);

		}
	}

	void flip()
	{
		print ("flip");
		m_isFacingRight = !m_isFacingRight;
	}
}
