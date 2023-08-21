using Microsoft.MixedReality.Toolkit.UI;
using Unity.Netcode;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public Transform modelToSpawn;
    [SerializeField] Transform loadingBalls;

    public void ObjectPlacer(Vector3 location)
    {
        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position + cam.transform.forward;
        this.gameObject.SetActive(false);
        Transform newLoadingObj = Instantiate(loadingBalls, pos, Quaternion.identity);
        newLoadingObj.GetComponent<ProgressIndicatorOrbsRotator>().OpenAsync();
        NetworkManager.Singleton.GetComponent<StartLesson>()
            .CreateClass(modelToSpawn, newLoadingObj, location, this.gameObject);
    }

    public void SetModelToSpawn(Transform model)
    {
        modelToSpawn = model;
    }
}