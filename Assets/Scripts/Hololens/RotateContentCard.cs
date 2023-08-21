using UnityEngine;

public class RotateContentCard : MonoBehaviour
{
    [SerializeField] private GameObject objToRotate; //Obj to rotate when card is clicked
    [SerializeField] private float speed = 100; //Rotation speed
    [SerializeField] public Transform modelToSpawn; //Model to spawn if this card is selected

    //Executed when the card is enabled
    void OnEnable()
    {
        foreach (var component in GameObject.FindObjectsOfType<RotateContentCard>())
        {
            //Disable the other rotation card if present and reset its rotation
            if (component != this.GetComponent<RotateContentCard>())
            {
                component.enabled = false;
                component.objToRotate.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
    }

    //Rotate card when selected
    private void Update()
    {
        objToRotate.transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}