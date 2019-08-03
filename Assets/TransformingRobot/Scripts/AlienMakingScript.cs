using UnityEngine;
using System.Collections;

public class AlienMakingScript : MonoBehaviour {
	public GameObject alienCamera;
	
	public GameObject[] armPrefabs;
	public GameObject bodyPrefab;
	public GameObject[] headPrefabs;
	public GameObject[] legPrefabs;
	public GameObject[] shellPrefabs;
	public GameObject[] tailPrefabs;

	public GameObject alienArm;
	public GameObject alienBody;
	public GameObject alienHead;
	public GameObject alienLeg;
	public GameObject alienShell;
	public GameObject alienTail;

	GameObject rootBone;

	public int headNumber=1;
	public int armNumber=1;
	public int tailNumber=1;
	public int shellNumber=1;
	public int legNumber=9;

	float alienSize=10f;
	float armJointSize=1f;	
	float headJointSize=1f;
	float shellJointSize=1f;
	float tailJointSize=1f;
	float handSize=1f;
	float headSize=1f;
	float eyeSize=1f;
	float shellCenterSize=1f;
	float shellFrontSize=1f;	
	float shellBackSize=1f;
	float shellSideSize=1f;
	float shellExtraSize=1f;	
	float tailTipSize=1f;
	float alienSpeed=1f;
	float alienMass=1f;
	float jumpSpeed=8f;

	float minAlienSize=1f;
	float minArmJointSize=.5f;	
	float minHeadJointSize=.5f;
	float minShellJointSize=.5f;
	float minTailJointSize=.3f;
	float minHandSize=.5f;
	float minHeadSize=.5f;
	float minEyeSize=.5f;
	float minShellCenterSize=.5f;
	float minShellFrontSize=.5f;	
	float minShellBackSize=.5f;
	float minShellSideSize=.5f;
	float minShellExtraSize=.5f;	
	float minTailTipSize=.5f;
	float minAlienSpeed=.5f;
	float minAlienMass=.5f;
	float minJumpSpeed=4f;

	float maxAlienSize=15f;
	float maxArmJointSize=2f;	
	float maxHeadJointSize=2f;
	float maxShellJointSize=2f;
	float maxTailJointSize=2f;
	float maxHandSize=2f;
	float maxHeadSize=2f;
	float maxEyeSize=1f;
	float maxShellCenterSize=2f;
	float maxShellFrontSize=2f;	
	float maxShellBackSize=2f;
	float maxShellSideSize=2f;
	float maxShellExtraSize=2f;	
	float maxTailTipSize=2f;
	float maxAlienSpeed=2f;
	float maxAlienMass=2f;
	float maxJumpSpeed=16f;

	bool headColliderEnabled=true;
	
	void Start () {
		alienLeg=(GameObject)Instantiate (legPrefabs[legNumber],transform.position,transform.rotation);

		rootBone = alienLeg.GetComponent<AlienCreatureCharacter> ().rootBone;

		SetBody ();
		SetArm (armNumber);
		SetHead (headNumber);
		SetShell (shellNumber);
		SetTail (tailNumber);

		alienLeg.transform.localScale = new Vector3 (alienSize, alienSize, alienSize);
		alienCamera.GetComponent<AlienCreatureCameraScript> ().target = alienLeg;
		SetMass (alienMass);
		SetAnimatorSpeed (alienSpeed);
		SetJumpSpeed (jumpSpeed);
	}

	public void RandomSpawn(){
		headNumber=Random.Range(0,19);
		armNumber=Random.Range(0,8);;
		tailNumber=Random.Range(0,13);;
		shellNumber=Random.Range(0,14);;
		legNumber=Random.Range(0,9);;
		
		alienSize=Random.Range(minAlienSize,maxAlienSize);
		armJointSize=Random.Range(minArmJointSize,maxArmJointSize);	
		headJointSize=Random.Range(minHeadJointSize,maxHeadJointSize);
		shellJointSize=Random.Range(minShellJointSize,maxShellJointSize);
		tailJointSize=Random.Range(minTailJointSize,maxTailJointSize);
		handSize=Random.Range(minHandSize,maxHandSize);
		headSize=Random.Range(minHeadSize,maxHeadSize);
		eyeSize=Random.Range(minEyeSize,maxEyeSize);
		shellCenterSize=Random.Range(minShellCenterSize,maxShellCenterSize);
		shellFrontSize=Random.Range(minShellFrontSize,maxShellFrontSize);
		shellBackSize=Random.Range(minShellBackSize,maxShellBackSize);
		shellSideSize=Random.Range(minShellSideSize,maxShellSideSize);
		shellExtraSize=Random.Range(minShellExtraSize,maxShellExtraSize);	
		tailTipSize=Random.Range(minTailTipSize,maxTailTipSize);
		alienSpeed=Random.Range(minAlienSpeed,maxAlienSpeed);
		alienMass=Random.Range(minAlienMass,maxAlienMass);
		jumpSpeed=Random.Range(minJumpSpeed,maxJumpSpeed);
		Spawn ();
	}

	public void Spawn(){
		alienLeg.GetComponent<AlienCreatureUserController> ().enabled = false;
		alienLeg.GetComponent<AlienCreatureAIController> ().enabled = true;

		alienLeg=(GameObject)Instantiate (legPrefabs[legNumber],transform.position+Random.Range(-15f,15f)*Vector3.right+Random.Range(-15f,15f)*Vector3.back,transform.rotation);

		rootBone = alienLeg.GetComponent<AlienCreatureCharacter> ().rootBone;

		alienBody = null;
		alienArm = null;
		alienShell = null;
		alienArm = null;
		alienHead = null;
		alienTail = null;

		SetBody ();
		SetArm (armNumber);
		SetHead (headNumber);
		SetShell (shellNumber);
		SetTail (tailNumber);
		
		alienLeg.transform.localScale = new Vector3 (alienSize, alienSize, alienSize);
		alienCamera.GetComponent<AlienCreatureCameraScript> ().target = alienLeg;
	}

	public void SetBody(){
		if (alienBody!=null) {
			GameObject.Destroy (alienBody);
		}
		alienBody=(GameObject)Instantiate (bodyPrefab,rootBone.transform.position,rootBone.transform.rotation);
		alienBody.transform.parent = rootBone.transform;
		alienBody.transform.localScale = new Vector3 (1f, 1f, 1f);

	}

	public void SetArm(int armNum){
		armNumber = armNum;
		if (alienArm!=null) {
			GameObject.Destroy (alienArm);
		}
		alienArm=(GameObject)Instantiate (armPrefabs[armNum],rootBone.transform.position,rootBone.transform.rotation);
		alienArm.transform.parent = rootBone.transform;
		alienArm.transform.localScale = new Vector3 (1f, 1f, 1f);
		alienLeg.GetComponent<AlienCreatureCharacter> ().armAnimator = alienArm.GetComponent<Animator> ();

		SetArmJointSize (armJointSize);
		SetHandSize (handSize);
		alienLeg.GetComponent<AlienCreatureCharacter> ().armAnimator.speed = alienSpeed;
	}

	public void SetHead(int headNum){
		headNumber = headNum;
		if (alienHead!=null) {
			GameObject.Destroy (alienHead);
		}
		alienHead=(GameObject)Instantiate (headPrefabs[headNum],rootBone.transform.position,rootBone.transform.rotation);
		alienHead.transform.parent = rootBone.transform;
		alienHead.transform.localScale = new Vector3 (1f, 1f, 1f);
		alienLeg.GetComponent<AlienCreatureCharacter> ().headAnimator = alienHead.GetComponent<Animator> ();
		alienLeg.GetComponent<AlienCreatureCharacter> ().alienHead = alienHead;
		alienLeg.GetComponent<AlienCreatureCharacter> ().headAnimator.speed = alienSpeed;

		SetHeadJointSize (headJointSize);
		SetHeadSize (headSize);
		SetEyeSize (eyeSize);
		EnableHeadCollider (headColliderEnabled);
	}

	public void SetLeg(int legNum){
		legNumber = legNum;
		if (alienLeg!=null) {
			GameObject.Destroy (alienLeg);
		}
		alienLeg=(GameObject)Instantiate (legPrefabs[legNum],transform.position,transform.rotation);
		alienLeg.transform.localScale = new Vector3 (alienSize, alienSize, alienSize);
		alienLeg.GetComponent<AlienCreatureCharacter> ().legAnimator = alienLeg.GetComponent<Animator> ();
		rootBone = alienLeg.GetComponent<AlienCreatureCharacter> ().rootBone;

		alienBody.transform.position = rootBone.transform.position;
		alienBody.transform.rotation = rootBone.transform.rotation;
		alienBody.transform.parent = rootBone.transform;
		alienBody.transform.localScale = new Vector3 (1f, 1f, 1f);

		alienShell.transform.position = rootBone.transform.position;
		alienShell.transform.rotation = rootBone.transform.rotation;
		alienShell.transform.parent = rootBone.transform;
		alienShell.transform.localScale = new Vector3 (1f, 1f, 1f);

		alienHead.transform.position = rootBone.transform.position;
		alienHead.transform.rotation = rootBone.transform.rotation;
		alienHead.transform.parent = rootBone.transform;
		alienHead.transform.localScale = new Vector3 (1f, 1f, 1f);
		alienLeg.GetComponent<AlienCreatureCharacter> ().headAnimator = alienHead.GetComponent<Animator> ();

		alienArm.transform.position = rootBone.transform.position;
		alienArm.transform.rotation = rootBone.transform.rotation;
		alienArm.transform.parent = rootBone.transform;
		alienArm.transform.localScale = new Vector3 (1f, 1f, 1f);
		alienLeg.GetComponent<AlienCreatureCharacter> ().armAnimator = alienArm.GetComponent<Animator> ();

		alienTail.transform.position = rootBone.transform.position;
		alienTail.transform.rotation = rootBone.transform.rotation;
		alienTail.transform.parent = rootBone.transform;
		alienTail.transform.localScale = new Vector3 (1f, 1f, 1f);
		alienLeg.GetComponent<AlienCreatureCharacter> ().tailAnimator = alienTail.GetComponent<Animator> ();
		SetMass (alienMass);
		SetAnimatorSpeed (alienSpeed);
		SetJumpSpeed (jumpSpeed);
		alienCamera.GetComponent<AlienCreatureCameraScript> ().target = alienLeg;
	}

	public void SetShell(int shellNum){
		shellNumber = shellNum;
		if (alienShell!=null) {
			GameObject.Destroy (alienShell);
		}
		alienShell=(GameObject)Instantiate (shellPrefabs[shellNum],rootBone.transform.position,rootBone.transform.rotation);
		alienShell.transform.parent = rootBone.transform;
		alienShell.transform.localScale = new Vector3 (1f, 1f, 1f);

		alienLeg.GetComponent<AlienCreatureCharacter> ().alienShell = alienShell;
		SetShellJointSize (shellJointSize);
		SetShellCenterSize (shellCenterSize);
		SetShellBackSize (shellBackSize);
		SetShellFrontSize(shellFrontSize);
		SetShellSideSize(shellSideSize);
		SetShellExtraSize (shellExtraSize);
		
	}

	public void SetTail(int taillNum){
		tailNumber = taillNum;
		if (alienTail!=null) {
			GameObject.Destroy (alienTail);
		}
		alienTail=(GameObject)Instantiate (tailPrefabs[taillNum],rootBone.transform.position,rootBone.transform.rotation);
		alienTail.transform.parent = rootBone.transform;
		alienTail.transform.localScale = new Vector3 (1f, 1f, 1f);
		alienLeg.GetComponent<AlienCreatureCharacter> ().tailAnimator = alienTail.GetComponent<Animator> ();
		alienLeg.GetComponent<AlienCreatureCharacter> ().alienTail = alienTail;
		alienLeg.GetComponent<AlienCreatureCharacter> ().tailAnimator.speed = alienSpeed;

		SetTailJointSize (tailJointSize);
		SetTailTipSize (tailTipSize);
		SetAnimatorSpeed (alienSpeed);
	}


	public void SetAlienSize(float aSize){
		alienSize = aSize;
		alienLeg.transform.localScale = new Vector3 (aSize,aSize,aSize);
		alienLeg.GetComponent<AlienCreatureCharacter> ().groundedCheckDistance = (aSize / 10f) * alienLeg.GetComponent<AlienCreatureCharacter> ().originalGroundedCheckDistance;
	}

	public void SetArmJointSize(float aSize){
		armJointSize = aSize;
		alienArm.GetComponent<AlienArmScript> ().armJointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
		alienBody.GetComponent<AlienBodyScript> ().armJointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
	}

	public void SetHeadJointSize(float aSize){
		headJointSize = aSize;
		alienHead.GetComponent<AlienHeadScript> ().headJointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
		alienBody.GetComponent<AlienBodyScript> ().headJointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
	}

	public void SetShellJointSize(float aSize){
		shellJointSize = aSize;
		alienShell.GetComponent<AlienShellScript> ().JointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
		alienBody.GetComponent<AlienBodyScript> ().shellJointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
	}

	public void SetTailJointSize(float aSize){
		tailJointSize = aSize;
		alienTail.GetComponent<AlienTailScript> ().tailJointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
		alienBody.GetComponent<AlienBodyScript> ().tailJointBone.transform.localScale = new Vector3 (aSize,aSize,aSize);;
	}

	public void SetHandSize(float aSize){
		handSize = aSize;
		foreach (GameObject lefthand in alienArm.GetComponent<AlienArmScript>().leftHandBones) {
			lefthand.transform.localScale=new Vector3 (aSize,aSize,aSize);
		}
		foreach (GameObject lefthand in alienArm.GetComponent<AlienArmScript>().rightHandBones) {
			lefthand.transform.localScale=new Vector3 (aSize,aSize,aSize);
		}
	}

	public void SetHeadSize(float aSize){
		headSize = aSize;
		alienHead.GetComponent<AlienHeadScript>().headBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
	}

	public void SetEyeSize(float aSize){
		eyeSize = aSize;
		if (alienHead.GetComponent<AlienHeadScript> ().leftEyeBone != null) {
			alienHead.GetComponent<AlienHeadScript> ().leftEyeBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
			alienHead.GetComponent<AlienHeadScript> ().rightEyeBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
		}
	}

	public void SetShellCenterSize(float aSize){
		shellCenterSize = aSize;
		alienShell.GetComponent<AlienShellScript>().centerBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
	}

	public void SetShellFrontSize(float aSize){
		shellFrontSize = aSize;
		alienShell.GetComponent<AlienShellScript>().frontBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
	}

	public void SetShellBackSize(float aSize){
		shellBackSize = aSize;
		alienShell.GetComponent<AlienShellScript>().backBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
	}

	public void SetShellSideSize(float aSize){
		shellSideSize = aSize;
		alienShell.GetComponent<AlienShellScript>().leftBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
		alienShell.GetComponent<AlienShellScript>().rightBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
	}

	public void SetShellExtraSize(float aSize){
		shellExtraSize = aSize;
		foreach(GameObject extraBone in alienShell.GetComponent<AlienShellScript>().extraBones){
			extraBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
		}
	}

	public void SetTailTipSize(float aSize){
		tailTipSize = aSize;
		alienTail.GetComponent<AlienTailScript>().tailTipBone.transform.localScale=new Vector3 (aSize,aSize,aSize);
	}
	
	public void EnableHeadCollider(bool tf){
		headColliderEnabled = tf;
		alienHead.GetComponent<AlienHeadScript> ().headBone.GetComponent<Collider> ().enabled = tf;
	}
	
	public void SetAnimatorSpeed(float aSpeed){
		alienSpeed = aSpeed;
		alienLeg.GetComponent<AlienCreatureCharacter> ().armAnimator.speed = aSpeed;
		alienLeg.GetComponent<AlienCreatureCharacter> ().legAnimator.speed = aSpeed;
		alienLeg.GetComponent<AlienCreatureCharacter> ().headAnimator.speed = aSpeed;
		alienLeg.GetComponent<AlienCreatureCharacter> ().tailAnimator.speed = aSpeed;
	}

	public void SetMass(float aMass){
		alienMass = aMass;
		alienLeg.GetComponent<Rigidbody> ().mass = aMass;
	}

	public void SetJumpSpeed(float aJumpSpeed){
		jumpSpeed = aJumpSpeed;
		alienLeg.GetComponent<AlienCreatureCharacter> ().jumpSpeed = aJumpSpeed;
	}
}
