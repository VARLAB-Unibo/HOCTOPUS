using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManageToggle : NetworkBehaviour
{
    //Objects to enable/disable with toggle
    [SerializeField] private List<GameObject> listOfObjToActivate;

    //Called by server in ActivateToggle, we dont call it in client
    public void ActiveDeactivateObj(bool isToggle, GameObject objToActivate)
    {
        RemoveOutlineIfPresent(objToActivate);

        //We need to take the index because we cant pass the gameobj to the clientrpc function
        int objIndex = GetIndexFromObj(objToActivate);

        //Main function to activate/deactivate the obj. Here we are executing the code in server
        ActivateDeactivate(isToggle, objIndex);

        //We execute the code in every client
        ActivateToggleClientRpc(isToggle, objIndex);
    }

    private void RemoveOutlineIfPresent(GameObject objToCheck)
    {
        if (objToCheck.transform.tag == "Layer") {
            foreach (Transform specificPart in objToCheck.transform) {
                RemoveOutlineIfPresent(specificPart.gameObject);
            }
        }
        else { 
            foreach (Transform specificPart in objToCheck.transform)
            {
                Outline outlineComponent = specificPart.GetComponent<Outline>();

                if (outlineComponent != null && outlineComponent.enabled)
                {
                    this.GetComponent<ManageOutline>().RemoveOutline(specificPart.gameObject);
                    RemoveOutlineIfPresentClientRpc();
                }
            }
        }
    }

    [ClientRpc]
    private void RemoveOutlineIfPresentClientRpc()
    {
        GameObject student = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        student.GetComponent<ClientHandler>().RemoveOutline(true);
    }

    //Code executed in every client
    [ClientRpc]
    public void ActivateToggleClientRpc(bool isToggle, int objIndex, ClientRpcParams clientRpcParams = default)
    {
        //It calls the main function directly
        ActivateDeactivate(isToggle, objIndex);


        //Update specific parts menu
        GameObject student = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
        student.GetComponent<CheckActiveParts>().UpdateSelectPanelParts();
    }

    //Base function for enabling and disabling the obj called from the server and also from the clients
    private void ActivateDeactivate(bool isToggle, int objIndex)
    {
        //We take the true obj from the serialized list and we activate/deactivate it
        GameObject obj = GetObjFromIndex(objIndex);
        obj.gameObject.SetActive(isToggle);
    }


    public void ActivateToggleSpecificClient(bool isToggle, GameObject objToActivate, ulong clientID)
    {
        //We need to take the index because we cant pass the gameobj to the clientrpc function
        int objIndex = GetIndexFromObj(objToActivate);
        //We execute the code in every client
        ActivateToggleClientRpc(isToggle, objIndex, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID },
            }
        });
    }


    //Simple functions for taking the index of the obj from the list and viceversa
    private int GetIndexFromObj(GameObject obj)
    {
        return listOfObjToActivate.IndexOf(obj);
    }

    private GameObject GetObjFromIndex(int index)
    {
        return listOfObjToActivate[index];
    }

    public List<GameObject> GetParts() {
        return listOfObjToActivate;
    }
}