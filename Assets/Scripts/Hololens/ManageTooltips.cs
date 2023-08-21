using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManageTooltips : NetworkBehaviour
{
    //Container of toggles for enable/disable tooltips
    [SerializeField] private GameObject containerTooltipsToggle;

    //Obj with tooltips attached
    [SerializeField] private List<GameObject> listOfObjWithTooltips;


    //Called by server in ActivateTooltips, we dont call it in client
    public void ActiveDeactivateTooltips(GameObject objWithTooltips)
    {
        //We need to take the index because we cant pass the gameobj to the clientrpc function
        int objIndex = GetIndexFromObj(objWithTooltips);

        //Main function to activate/deactivate the obj
        bool wasActive = CheckActive(objIndex);

        //We execute the base function on server
        ActivateDeactivateBase(objIndex, wasActive);

        //We excecute the code in every client
        ActivateToggleClientRpc(objIndex, wasActive);
    }

    //Code excecuted in every client
    [ClientRpc]
    private void ActivateToggleClientRpc(int objIndex, bool wasActive, ClientRpcParams clientRpcParams = default)
    {
        //It calls the main function directly
        ActivateDeactivateBase(objIndex, wasActive);
    }

    public void ActivateToggleSpecificClient(ulong clientID)
    {
        foreach (var objWithTooltip in listOfObjWithTooltips)
        {
            Transform tooltip = objWithTooltip.transform.Find("Tooltips");
            if (tooltip.gameObject.activeSelf)
            {
                //We need to take the index because we cant pass the gameobj to the clientrpc function
                int objIndex = GetIndexFromObj(objWithTooltip);

                //We execute the code in every client
                ActivateToggleClientRpc(objIndex, false, new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { clientID },
                    }
                });
            }
        }
    }

    //Check if the tooltips are enabled or not
    private bool CheckActive(int objIndex)
    {
        GameObject obj = GetObjFromIndex(objIndex);
        bool wasActive = obj.transform.Find("Tooltips").gameObject.activeSelf;
        containerTooltipsToggle.GetComponent<OnlyOneLabelToggle>().CheckToggle(wasActive);
        return wasActive;
    }

    //Base function for enabling and disabling tooltips, called from the server and also from the clients
    private void ActivateDeactivateBase(int objIndex, bool wasActive)
    {
        GameObject obj = GetObjFromIndex(objIndex);

        Transform tooltipsContainer = obj.transform.Find("Tooltips");

        //If it was active we need to search the tooltips of our obj and deactivate them
        if (!wasActive)
        {
            foreach (var objWithTooltip in listOfObjWithTooltips)
            {
                if (objWithTooltip != obj)
                {
                    Transform tooltip = objWithTooltip.transform.Find("Tooltips");
                    tooltip.gameObject.SetActive(false);
                }
            }
        }

        tooltipsContainer.gameObject.SetActive(!wasActive);
    }

    //Simple functions for taking the index of the obj from the list and viceversa
    private int GetIndexFromObj(GameObject obj)
    {
        return listOfObjWithTooltips.IndexOf(obj);
    }

    private GameObject GetObjFromIndex(int index)
    {
        return listOfObjWithTooltips[index];
    }
}