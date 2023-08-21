using UnityEngine;

public class MoveSpawnedObj : MonoBehaviour
{
    //Model we want to observe. We change its scale when pinching and we move its position when dragging
    private GameObject spawnedObject;

    //PinchToZoom variables
    private float initialDistance;
    private Vector3 initialScale;

    //DragToMove variables
    private Touch touch;
    private float speedModifier = 0.001f;


    void Start()
    {
        spawnedObject = GameObject.FindGameObjectWithTag("SpawnedModel");
    }


    // Update is called once per frame
    void Update()
    {
        checkPinchToZoom();
        checkDragToMove();
    }

    //Check if the client is pinching. When is pinching we change (only in this client) the scale of the model
    private void checkPinchToZoom()
    {
        if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            // if any one of touchzero or touchOne is cancelled or maybe ended then do nothing
            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return; // basically do nothing
            }

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = spawnedObject.transform.localScale;
            }
            else // if touch is moved
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                //if accidentally touched or pinch movement is very very small
                if (Mathf.Approximately(initialDistance, 0))
                {
                    return; // do nothing if it can be ignored where inital distance is very close to zero
                }

                var factor = currentDistance / initialDistance;

                // scale multiplied by the factor we calculated
                spawnedObject.transform.localScale = initialScale * factor;
            }
        }
    }

    //Check if the client is dragging. When is dragging we change (only in this client) the position of the model
    private void checkDragToMove()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                spawnedObject.transform.position = new Vector3(
                    spawnedObject.transform.position.x + touch.deltaPosition.x * speedModifier,
                    spawnedObject.transform.position.y,
                    spawnedObject.transform.position.z + touch.deltaPosition.y * speedModifier);
            }
        }
    }
}