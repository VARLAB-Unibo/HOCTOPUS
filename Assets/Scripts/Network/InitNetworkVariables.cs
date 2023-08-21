using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Unity.Netcode;
using UnityEngine;

public class InitNetworkVariables : NetworkBehaviour
{
    [SerializeField] private GameObject toggleContainer;

    private NetworkVariable<Quaternion> diffRotation = new NetworkVariable<Quaternion>(Quaternion.identity);

    private void Start()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += (clientID) =>
        {
            InitToggles(clientID);
            ManageSlider manageSliderComponent = this.GetComponent<ManageSlider>();

            if (manageSliderComponent != null)
                manageSliderComponent.ChangeColor();

            InitLabels(clientID);
        };
    }

    private void InitToggles(ulong clientID)
    {
        List<GameObject> parts = this.GetComponent<ManageToggle>().GetParts();

        foreach (GameObject part in parts) {
            if (!part.activeSelf)
            {
                this.GetComponent<ManageToggle>().ActivateToggleSpecificClient(false, part, clientID);
            }        
        }
    }

    private void InitLabels(ulong clientID)
    {
        this.GetComponent<ManageTooltips>().ActivateToggleSpecificClient(clientID);
    }

    public void AddDiffRotat(Quaternion newRotationToShare)
    {
        diffRotation.Value *= newRotationToShare;
    }

    public void SetDiffRotat(Quaternion newDiffRotation)
    {
        diffRotation.Value = newDiffRotation;
    }

    public Quaternion GetDiffRotation()
    {
        return diffRotation.Value;
    }
}