using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWaypoint : MonoBehaviour {

	public List<Transform> waypoints;
	public List<Vector3> WpPositions;
	public Transform MyTransform;
	public float PathDuration = 25f;
	public float lookAhead = 0.01f;
	public Vector3 UpVector;
	public bool closed = false;
	private Vector3 velocity;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < waypoints.Count; i++){
			WpPositions.Add(waypoints[i].position);
		}
		StartCoroutine(FollowWay());
		//MyTransform.DOPath(WpPositions.ToArray(), PathDuration,PathType.CatmullRom,PathMode.Full3D,10,Color.red).SetLoops(-1,LoopType.Restart).SetLookAt(lookAhead,UpVector).SetEase(Ease.Linear);
	}

	IEnumerator FollowWay(){
		MyTransform.position = waypoints[0].position;

		foreach(Transform t in waypoints){
			float start = Time.time;
			while(Vector3.Distance(MyTransform.position,t.position) > 0.01f){
				//MyTransform.position = Vector3.Slerp(MyTransform.position, t.position,(Time.time-start)/5f);
				//Quaternion targetRotation = Quaternion.LookRotation(MyTransform.position+((t.position-MyTransform.position).normalized*lookAhead) - MyTransform.position);
				Quaternion targetRotation = Quaternion.LookRotation(t.position - MyTransform.position);
				// Smoothly rotate towards the target point.
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 6f * Time.deltaTime);
				//MyTransform.LookAt(MyTransform.position+((t.position-MyTransform.position).normalized*lookAhead));
				//MyTransform.position = Vector3.SmoothDamp(MyTransform.position, t.position, ref velocity, 3f);
				MyTransform.position = Vector3.Lerp(MyTransform.position, t.position, (Time.time-start)/PathDuration);
				yield return null;
			}
		}
		StartCoroutine(FollowWay());
		yield return null;
	}
}
