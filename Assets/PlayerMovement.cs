using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
	public bool useControllerInput = false, pointerDown = false;
	public float moveSpeed = 200f, smoothTime, accelTime, dashDistance, dashCooldown;
	public Cinemachine.CinemachineVirtualCamera playerCam;
	[SerializeField] AnimationCurve accelerationCurve;

	bool canDash = true;
	int xInverted = -1, yInverted = -1;
	Rigidbody rb;
	Vector3 dir = Vector3.zero, oldDir = Vector3.zero, dirSnapshot, flatAccel;
	Matrix4x4 calibrationMatrix;
	ParticleSystemDictionary particleSystems;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		CalibrateAccelerometer();
		particleSystems = GetComponentInChildren<ParticleSystemDictionary>();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Joystick1Button0) && canDash){
			Dash();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 currentVelocityVector = rb.velocity;
		float currentVelocity = currentVelocityVector.magnitude;

		if(!useControllerInput){
			Vector3 rawDir = Input.acceleration;
			Vector3 fixedDir = calibrationMatrix.MultiplyVector(rawDir);
			if(fixedDir.sqrMagnitude > 1) fixedDir.Normalize();

			Vector3.SmoothDamp(oldDir, fixedDir, ref dir, smoothTime, moveSpeed);
			flatAccel = new Vector3(dir.x * xInverted, 0f, dir.y * yInverted);

			if(pointerDown) flatAccel = Vector3.zero;
		} else {
			Vector3 rawDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
			Debug.Log(rawDir + " = " + rawDir.magnitude);
			if(rawDir.sqrMagnitude > 1) rawDir.Normalize();
			Vector3.SmoothDamp(oldDir, rawDir, ref dir, smoothTime, moveSpeed);
			flatAccel = new Vector3(dir.x * xInverted, 0f, -dir.y * yInverted);
		}

		//Debug.Log(flatAccel + " = " + flatAccel.magnitude);
		
		//rb.MovePosition(transform.position + flatAccel * Mathf.SmoothDamp(currentVelocity, moveSpeed * accelerationCurve.Evaluate(flatAccel.magnitude), ref currentVelocity, accelTime));
		rb.AddForce(flatAccel * Mathf.SmoothDamp(currentVelocity, moveSpeed * accelerationCurve.Evaluate(flatAccel.magnitude), ref currentVelocity, accelTime), ForceMode.Acceleration);
		Debug.DrawRay(transform.position + Vector3.up, rb.velocity, Color.red, Time.fixedDeltaTime);
		transform.LookAt(transform.position + flatAccel);
		
		oldDir = dir;
	}

	public void CalibrateAccelerometer(){
		dirSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), dirSnapshot);

		Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, Vector3.one);

		calibrationMatrix = matrix.inverse;
	}

	public void SetPointerState(bool pointerDown){
		this.pointerDown = pointerDown;
	}

	public void ToggleXAxis(bool inverted){
		xInverted = inverted ? 1 : -1;
	}
	public void ToggleYAxis(bool inverted){
		yInverted = inverted ? 1 : -1;
	}

	public void Dash(){
		canDash = false;
		rb.velocity = Vector3.zero;
		Vector3 dashVelocity = Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime)));
		this.Freeze(Time.fixedUnscaledDeltaTime * 2f);
		rb.AddForce(dashVelocity, ForceMode.VelocityChange);
		particleSystems.availableParticles["p_dash"].Play();
		StartCoroutine(DashTimer());
	}

	IEnumerator DashTimer(){
		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}

}
