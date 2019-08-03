using UnityEngine;
using System.Collections;

public class AlienCreatureCharacter : MonoBehaviour {
	public GameObject rootBone;
	public GameObject alienHead;
	public GameObject alienTail;
	public GameObject alienArm;
	public GameObject alienShell;

	public Animator legAnimator;
	public Animator headAnimator;
	public Animator tailAnimator;
	public Animator armAnimator;

	public float groundedCheckDistance=1f;
	public float originalGroundedCheckDistance=1f;

	public bool isGrounded=false;
	public bool jumpUp=false;

	public float jumpSpeed=5f;
	
	void Awake(){
		legAnimator = GetComponent<Animator> ();
	}

	void FixedUpdate(){
		GroundedCheck ();
	}

	public void GroundedCheck(){
		if (legAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Fall") {
			jumpUp=false;
			legAnimator.SetBool ("Jumping", false);
			tailAnimator.SetBool ("Jumping", false);
			headAnimator.SetBool ("Jumping", false);
			armAnimator.SetBool ("Jumping", false);
		}

		if (!jumpUp) {
			if (Physics.Raycast (transform.position, Vector3.down, groundedCheckDistance)) {
				isGrounded = true;
				legAnimator.SetBool ("IsGrounded", true);
				tailAnimator.SetBool ("IsGrounded", true);
				headAnimator.SetBool ("IsGrounded", true);
				armAnimator.SetBool ("IsGrounded", true);
				legAnimator.applyRootMotion=true;
			} else {
				isGrounded = false;
				legAnimator.SetBool ("IsGrounded", false);
				tailAnimator.SetBool ("IsGrounded", false);
				headAnimator.SetBool ("IsGrounded", false);
				armAnimator.SetBool ("IsGrounded", false);
			}
		}
	}

	public void Attack(){
		legAnimator.SetTrigger ("Attack");
		tailAnimator.SetTrigger ("Attack");
		headAnimator.SetTrigger ("Attack");
		armAnimator.SetTrigger ("Attack");
	}
	
	public void Hit(){
		legAnimator.SetTrigger ("Hit");
		tailAnimator.SetTrigger ("Hit");
		headAnimator.SetTrigger ("Hit");
		armAnimator.SetTrigger ("Hit");
	}
	
	public void Jump(){
		if (isGrounded) {
			jumpUp=true;
			legAnimator.applyRootMotion = false;
			GetComponent<Rigidbody> ().AddForce ((transform.forward + transform.up) * jumpSpeed, ForceMode.Impulse);

			legAnimator.SetBool ("Jumping", true);
			tailAnimator.SetBool ("Jumping", true);
			headAnimator.SetBool ("Jumping", true);
			armAnimator.SetBool ("Jumping", true);

			isGrounded = false;
			legAnimator.SetBool ("IsGrounded", false);
			tailAnimator.SetBool ("IsGrounded", false);
			headAnimator.SetBool ("IsGrounded", false);
			armAnimator.SetBool ("IsGrounded", false);
		}

	}

	public void Down(){
		legAnimator.SetBool ("Down",true);
		tailAnimator.SetBool ("Down",true);
		headAnimator.SetBool ("Down",true);
		armAnimator.SetBool ("Down",true);
	}

	public void StandUp(){
		legAnimator.SetBool ("Down",false);
		tailAnimator.SetBool ("Down",false);
		headAnimator.SetBool ("Down",false);
		armAnimator.SetBool ("Down",false);
	}
	
	public void Move(float v,float h){
		legAnimator.SetFloat ("Forward",v);
		legAnimator.SetFloat ("Turn",h);	
		tailAnimator.SetFloat ("Forward",v);
		tailAnimator.SetFloat ("Turn",h);	
		headAnimator.SetFloat ("Forward",v);
		headAnimator.SetFloat ("Turn",h);	
		armAnimator.SetFloat ("Forward",v);
		armAnimator.SetFloat ("Turn",h);	
	}
}
