using UnityEngine;

public class ResetModelPosition : MonoBehaviour
{
    private Camera cameraScene; //Client's main camera
    private Vector3 startingModelScale; //Starting scale of the model, we save it for reset when needed
    private float startHeight; //Starting scale of the model, we save it for reset when needed

    void Start()
    {
        startingModelScale = this.transform.localScale;
        startHeight = this.transform.position.y;
    }

    //Reset position and scale of the model. We put it in front of the client's camera, looking at him. 
    public void RepositionModel(bool isToRotate)
    {
        cameraScene = Camera.main;
        this.transform.localScale = startingModelScale;
        this.transform.position = cameraScene.transform.position + cameraScene.transform.forward;

        this.transform.position = new Vector3(this.transform.position.x, startHeight, this.transform.position.z);

        if (isToRotate)
        {
            Debug.Log("I'm a teacher!");

            Vector3 relativePos = cameraScene.transform.position - this.transform.position;
            relativePos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            this.transform.rotation = rotation;

            this.gameObject.GetComponent<RotateModelForClient>().ResetStartingRotation();
        }
    }
}