using Unity.Netcode;
using UnityEngine;

public class FindModelAndReset : MonoBehaviour
{
    public void Reset()
    {
        GameObject model = GameObject.FindGameObjectWithTag("SpawnedModel");
        if (model != null)
        {
            bool isToRotate = NetworkManager.Singleton.IsServer;
            model.GetComponent<ResetModelPosition>().RepositionModel(isToRotate);
        }
        else
        {
            Debug.Log("Nessun modello trovato!");
        }
    }
}