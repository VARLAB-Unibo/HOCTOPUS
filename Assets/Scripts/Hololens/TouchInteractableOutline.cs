using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;


//Used for manage the activation of the outline
public class TouchInteractableOutline : MonoBehaviour, IMixedRealityTouchHandler
{
    #region Event handlers

    public TouchEvent OnTouchCompleted;
    public TouchEvent OnTouchStarted;
    public TouchEvent OnTouchUpdated;

    #endregion

    //We remove the outline when we finish to touch the model
    void IMixedRealityTouchHandler.OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        OnTouchCompleted.Invoke(eventData);
        GameObject.FindGameObjectWithTag("SpawnedModel").GetComponent<ManageOutline>().RemoveOutline(this.gameObject);
    }

    //We add the outline when we start to touch the model
    void IMixedRealityTouchHandler.OnTouchStarted(HandTrackingInputEventData eventData)
    {
        OnTouchStarted.Invoke(eventData);
        GameObject.FindGameObjectWithTag("SpawnedModel").GetComponent<ManageOutline>().AddOutline(this.gameObject);
    }

    void IMixedRealityTouchHandler.OnTouchUpdated(HandTrackingInputEventData eventData)
    {
        OnTouchUpdated.Invoke(eventData);
    }
}