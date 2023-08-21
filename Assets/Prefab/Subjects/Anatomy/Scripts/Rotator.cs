using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public Transform target;
	public Vector3 RotateVector = Vector3.up;
	public float Speed = 5f;

	void Update(){
		target.eulerAngles += RotateVector * Speed * Time.deltaTime;
	}
}
