using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchModelDetection : MonoBehaviour
{
    void Start()
    {
        GameObject model = GameObject.FindWithTag("SpawnedModel");
        model.GetComponent<BoxCollider>().enabled = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                this.GetComponent<ClientHandler>().ShowOutline(raycastHit.collider.name);
            }
        }
    }
}