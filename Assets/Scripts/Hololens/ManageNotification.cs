using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;
using UnityEngine;

public class ManageNotification : MonoBehaviour
{
    [SerializeField] private GameObject notificationPrefab; //Prefab of the notification
    [SerializeField] private GameObject notificationGroup; //GO where we put the notifications


    public void AddNotification(string studentName)
    {
        //Set notification text
        notificationPrefab.GetComponentInChildren<TextMeshPro>().text = studentName + " has a question!";

        //We instatiate the notification
        GameObject objSpawned = Instantiate(notificationPrefab, notificationGroup.transform);

        //UpdateCollection to visualize better the notifications
        notificationGroup.GetComponent<GridObjectCollection>().UpdateCollection();

        //Destroy the notification after 5 seconds   
        Destroy(objSpawned.gameObject, 5f);
    }
}