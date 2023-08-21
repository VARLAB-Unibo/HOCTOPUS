using Microsoft.MixedReality.Toolkit.UI;
using Unity.Netcode;
using UnityEngine;

public class ActivateToggle : MonoBehaviour
{
    [SerializeField] private GameObject objToActivate;
    [SerializeField] private GameObject toggle;
    [SerializeField] private GameObject root;

    // Call the true function in the root-->manageToggle
    public void ActivateToggleCall()
    {
        //We take the boolean value of the toggle and we pass it to the function with the gameobj to activate/deactivate
        bool isToggle = toggle.GetComponent<Interactable>().IsToggled;
        root.GetComponent<ManageToggle>().ActiveDeactivateObj(isToggle, objToActivate);
    }
    public GameObject GetObjToActivate()
    {
        return objToActivate;
    }
}

