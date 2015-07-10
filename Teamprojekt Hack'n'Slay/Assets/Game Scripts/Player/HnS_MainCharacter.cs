using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
public class HnS_MainCharacter : MonoBehaviour 
{
	Rigidbody m_Rigidbody;
	Animator m_Animator;

	private float m_goalVelocity;
	private const float m_horizontalMaxSpeed = 15.0f;
	private const float m_jumpPower = 10.0f;
	//private bool m_jumpRequest = false;
	//private float m_GroundCheckDistance = 0.5f;
	private bool m_isFacingRight;
	private Ray m_downRay; 
	private const float aerialMovDamping = 0.1f;
	private bool doubleJumpReady = true; 
	private float stopMovementDelay = 0;
	//private float m_currentAccelleration = 0;
	

	public RaycastHit m_downRayHit;
	public float m_totalGroundDistance = 0;
	public bool m_IsGrounded;
	public float m_height;
	public HnS_CameraScript m_mainCamera;



	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_isFacingRight = true;
		m_mainCamera.setTarget (this);
		m_height = this.transform.localScale.y * this.GetComponent<SphereCollider>().radius * 2;
		print ("MainCharacter m_height:" + m_height);
		//m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		// the bigger the difference in currentVelocity and goalVelocity, the stronger the accelleration will be. If equal, accelleration should be 0
		float diff = m_goalVelocity - m_Rigidbody.velocity.x;
		// Accelleration/Controls depends on ground and airborne-state
		//m_currentAccelleration = (m_IsGrounded) ? Mathf.Sign(diff)*diff*diff : Mathf.Sign(diff)*diff*diff*aerialMovDamping;
		m_Animator.SetFloat ("WalkSpeed", m_goalVelocity);

		print ("goalVelo: " + m_goalVelocity + "\nstopMovementDelay: " + stopMovementDelay);

		//print ("animator state: " + m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

		if ((m_goalVelocity == 0) && (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
		{
			if(stopMovementDelay > 0.2)
			{
				print ("RunStop");
				m_Animator.SetTrigger("RunStop");
				stopMovementDelay = 0;
			}
			stopMovementDelay += Time.deltaTime;
		} 
		else 
		{
			stopMovementDelay = 0;
		}

	}

	// h beschreibt Anteil an maximaler x-Geschwindigkeit in Intervall [-1, 1]
	public void move(float h, float v, bool jump)
	{
		CheckGroundStatus ();
		// check if input-direction is different to direction the character is facing
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

	}

	void HandleGroundedMovement (float h, bool jump)
	{
		if (jump) {

			print ("jump");
			m_Animator.SetTrigger("Jump");
			//this.transform.position = this.transform.position + new Vector3(0, m_jumpPower/5, 0);
		}
	}
	
	void HandleAirborneMovement (float h, float v, bool jump)
	{
		if (doubleJumpReady && jump) {
			print ("doubleJump BITCHES!");
			m_Animator.SetTrigger("DoubleJump");
			//m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_jumpPower, m_Rigidbody.velocity.z);
			doubleJumpReady = false;
		}
	}

	void CheckGroundStatus()
	{
		#if UNITY_EDITOR
		Debug.DrawLine(transform.position + new Vector3(0, m_height/2, 0), transform.position + new Vector3(0, m_height/2, 0) + (Vector3.down * m_height/2), Color.red, 1);
		#endif
	
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
		m_Animator.SetTrigger ("TurnAround");
		m_isFacingRight = !m_isFacingRight;
	}
}
