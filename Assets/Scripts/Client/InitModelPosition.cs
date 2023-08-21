using Unity.Netcode;
using UnityEngine;

public class InitModelPosition : NetworkBehaviour
{
    private GameObject model; //Contains the model to observe in scene
    private Camera cameraScene; //Client's main camera
    private Vector3 startingModelScale; //Starting scale of the model, we save it for reset when needed
    private Quaternion diffRotation;
    private float heightDiff = 0.2f;
    private int forwardDiff = 2;

    void Start()
    {
        if (!IsOwner) return;

        cameraScene = Camera.main;
        model = GameObject.FindGameObjectWithTag("SpawnedModel");
        startingModelScale = model.transform.localScale;
        RepositionModel();
    }

    //Reset position and scale of the model. We put it in front of the client's camera, looking at him. 
    public void RepositionModel()
    {
        model.transform.localScale = startingModelScale;
        model.transform.position = cameraScene.transform.position + cameraScene.transform.forward * forwardDiff;
        model.transform.position = new Vector3(model.transform.position.x, model.transform.position.y - heightDiff,
            model.transform.position.z);

        diffRotation = model.gameObject.GetComponent<InitNetworkVariables>().GetDiffRotation();
        ResetRotationClient(diffRotation);
    }

    public void ResetRotationClient(Quaternion diffRotation)
    {
        Vector3 relativePos = cameraScene.transform.position - model.transform.position;
        relativePos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        model.transform.rotation = rotation;
        model.transform.rotation *= diffRotation;
    }
}