using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MyRotatable : MonoBehaviour, IDragHandler {
	public Transform target;
	public float RotateSpeedX = 5f;
	public float RotateSpeedY = 5f;

	// Update is called once per frame
	public void OnDrag(PointerEventData dt) {
		target.eulerAngles += new Vector3(-dt.delta.y*Time.deltaTime*RotateSpeedX,-dt.delta.x*Time.deltaTime*RotateSpeedY,0f);
	}

	public void Reset(){
		target.eulerAngles = Vector3.zero;
	}
}
