using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ManageOutline : NetworkBehaviour
{
    //List of GO with an outline script attached
    [SerializeField] private List<GameObject> interactableObj;


    //Called from "AddOutline" or from "RemoveOutline"
    private void EnableDisableComponent(GameObject objToOutline, bool activate)
    {
        if (!IsServer) return;

        int index = GetIndexFromObj(objToOutline);
        EnableDisableBase(index, activate);
        EnableDisableClientRpc(index, activate);
    } 
    //We enable and disable the outline for all clients
    [ClientRpc]
    private void EnableDisableClientRpc(int index, bool activate)
    {
        EnableDisableBase(index, activate);
    }

    //Base function for enabling and disabling the outline called from the server and also from the clients
    private void EnableDisableBase(int index, bool activate)
    {
        GetObjFromIndex(index).GetComponent<Outline>().enabled = activate;
    }


    //Simple functions for taking the index of the obj from the list and viceversa
    private int GetIndexFromObj(GameObject obj)
    {
        return interactableObj.IndexOf(obj);
    }

    private GameObject GetObjFromIndex(int index)
    {
        return interactableObj[index];
    }


    //Called every time we want to disable the outline
    public void RemoveOutline(GameObject obj)
    {
        EnableDisableComponent(obj, false);
    }

    //Called every time we want to enable the outline
    public void AddOutline(GameObject obj)
    {
        EnableDisableComponent(obj, true);
    }
}