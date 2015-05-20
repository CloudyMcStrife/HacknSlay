using UnityEngine;
using System.Collections;

//will establish a BoxCollider2D object upon binding to an object
[RequireComponent (typeof (BoxCollider2D))] 
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;
	//insets for the bounds
	const float skinWidth= .015f;

	//how many rays are fired horizontally / vertically
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float maxClimbAngle = 80;
	float maxDescendAngle = 75;

	float horizontalRaySpacing;
	float verticalRaySpacing;



	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	public void Move (Vector3 velocity)
	{
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityOld = velocity;

		if (velocity.y < 0) {
			DescendSlope (ref velocity);
		}

		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}

		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);
	}

	void HorizontalCollisions(ref Vector3 velocity ) 
	{
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		//print ("3"+collisions.velocityOld);    //it is (0,0,0)
		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i );
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin , Vector2.right * directionX, rayLength,collisionMask);
			
			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength,Color.red);

			
			if (hit)
			{
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						//print ("3"+collisions.velocityOld);
						//print ("1"+velocity);
						velocity = collisions.velocityOld;      //seems to reset the velocity vector, since velocityOld was never initialized
						//print ("4"+collisions.velocityOld);
						//print ("2"+velocity);
					}
					float distanceToSlopeStart = 0;
					if(slopeAngle != collisions.slopeAngleOld){
						distanceToSlopeStart = hit.distance -skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope (ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart;
				}



				if(!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) *directionX;
					rayLength = hit.distance;  // so that you wont fall off a ledge
						
					if (collisions.climbingSlope)
					{
						velocity.y = Mathf.Tan( collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x);
					}
					//epi 3
					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}

				//w
				if ( slopeAngle > maxClimbAngle) 
				{
					print ("v:"+velocity);

					Vector2 velocityWji = new Vector2(velocity.x, velocity.y);



					print ("Wji:"+velocityWji);


					//Vector2 normalWj = -(hit.normal);
					//normalWj.Normalize ();

					Vector2 rayOriginWj = (directionX == -1) ? raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
					RaycastHit2D hitWj = Physics2D.Raycast (rayOriginWj , velocityWji, rayLength,collisionMask);

					Debug.DrawRay (rayOriginWj, velocityWji * rayLength*1000,Color.green);


					//you just have to mirror the velocityWj on the Y axis, omwtfbbq i'm so dense
					if (velocityWji.y > -100f)
					{
						collisions.wallLeft = collisions.left;
						collisions.wallRight = collisions.right;

						//float angleWj = 2 *Vector2.Angle(normalWj, velocityWji);
						collisions.velocityWj.Set(velocityWji.x *(-1), velocityWji.y,0); 

						print ("Wj:"+collisions.velocityWj);




					}

				}
			}
		}
		
	}

	void VerticalCollisions(ref Vector3 velocity ) 
	{
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOrigin = (directionY == -1) ?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin , Vector2.up * directionY, rayLength,collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength,Color.red);

			if (hit)
			{
				velocity.y = (hit.distance - skinWidth) *directionY;
				rayLength = hit.distance;  // so that you wont fall off a ledge

				// epi 4 reason for jittering is that the collision above us is impacting the y velocity but not the x velocity hence jittering, 
				// therefore this passage of code

				if (collisions.climbingSlope)
				{
					velocity.x = velocity.y / Mathf.Tan ( collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

				}

				//epi 3
				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign (velocity.x);
			rayLength = Mathf.Abs (velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbSlope (ref Vector3 velocity, float slopeAngle)
	{
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {

		
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
	    }
	}

	void DescendSlope(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
		
		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign(hit.normal.x) == directionX) {
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;
						
						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
		}

	void UpdateRaycastOrigins()
	{

		//Represents an axis aligned bounding box.
		
		//An axis-aligned bounding box, or AABB for short, is a box aligned with coordinate axes and fully enclosing some object. Because the box is never rotated with respect to the axes, it can be defined by just its center and extents, or alternatively by min and max points.
		Bounds bounds = collider.bounds;

		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo{
		public bool above, below;
		public bool left,right;

		//walljump
		public bool wallLeft, wallRight;

		public bool climbingSlope;
		public bool descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld; //didnt know whereelse to put it
		public Vector3 velocityWj;

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;
			//walljump
			wallLeft = wallRight = false;

			//velocityWj= Vector3.zero;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;

		}
	}
}
