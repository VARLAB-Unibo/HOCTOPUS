using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepMenuHandler : MonoBehaviour
{
    
    // Call the true function in the root-->manageToggle
    public void ActivateDeepMenu()
    {

        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            Transform camTransform = Camera.main.transform;
            Vector3 pos = camTransform.position + camTransform.forward / 2 - (camTransform.right / 4);
            Transform thisTransform = this.transform;
            thisTransform.position = pos;
            thisTransform.LookAt(camTransform);
            thisTransform.RotateAround(thisTransform.position, thisTransform.up, 180f);
        }
    }
}
