using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeactivateToggle : MonoBehaviour
{
    public void Deactivate()
    {
        GameObject handMenu = GameObject.Find("HandMenuInfo(Clone)");

        if (handMenu != null)
        {
            handMenu.GetComponent<HandMenuInfoHandler>().CloseMenuButtonClicked();
        }
    }
}