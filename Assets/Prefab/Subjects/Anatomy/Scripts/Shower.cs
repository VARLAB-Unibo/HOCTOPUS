using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shower : MonoBehaviour {

	public List<GameObject> list;
	public int index = 0;
	public GameObject OpenedObject;
	public Text ObjectName;
	
	public void Start(){
		Open(index);
	}

	public void Open(int i){
		if(OpenedObject != null){
			OpenedObject.SetActive(false);
		}
		index = i;
		OpenedObject = list[index];
		OpenedObject.SetActive(true);
		ObjectName.text = OpenedObject.name;
	}

	public void Previous(){
		if(index > 0){
			Open (index-1);
		}
		else{
			Open (list.Count-1);
		}
	}

	public void Next(){
		Open((index+1)%list.Count);
	}
}
