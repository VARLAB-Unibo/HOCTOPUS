using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class OnlyOneLabelToggle : MonoBehaviour
{
    [SerializeField] public List<Interactable> toggleList;
    private Interactable previousToggle = null;
    private int nToggleActivated = 0;

    public void CheckToggle(bool wasActive)
    {
        //If we disable the last one
        if (wasActive)
        {
            previousToggle = null;
            nToggleActivated = 0;
        }
        else //Check if there are others toggle activated
        {
            nToggleActivated++;

            if (nToggleActivated > 1)
            {
                previousToggle.IsToggled = false;
                nToggleActivated--;
            }

            foreach (var toggle in toggleList)
            {
                if (toggle.IsToggled)
                {
                    previousToggle = toggle;
                }
            }
        }
    }

    private void DisableOtherToggle(Interactable toggleActivated)
    {
        foreach (var toggle in toggleList)
        {
            if (toggle != toggleActivated)
            {
                toggle.IsToggled = false;
            }
        }
    }
}