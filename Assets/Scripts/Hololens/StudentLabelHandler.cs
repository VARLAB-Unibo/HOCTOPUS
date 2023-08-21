using Microsoft.MixedReality.Toolkit.UI;
using Unity.Netcode;
using UnityEngine;

public class StudentLabelHandler : MonoBehaviour
{
    [SerializeField] GameObject handButton;
    [SerializeField] MeshRenderer iconMeshRendererHand;
    [SerializeField] MeshRenderer iconMeshRendererBlock;
    [SerializeField] MeshRenderer backPlateHandButton;
    [SerializeField] MeshRenderer backPlateGrantPermissionButton;
    private ulong clientID;
    [SerializeField] Material materialYellow;
    [SerializeField] Material materialRed;
    [SerializeField] Material materialGreen;
    [SerializeField] Material materialWhite;
    [SerializeField] Material materialGrey;
    [SerializeField] Material materialRedButton;
    SendInfoClient modelSendInfo;
    bool flagPermit = false;

    private void Start()
    {
        modelSendInfo = GameObject.FindGameObjectWithTag("SpawnedModel").GetComponent<SendInfoClient>();
    }

    public void SetClientID(ulong id)
    {
        clientID = id;
    }

    public ulong GetClientID()
    {
        return clientID;
    }

    public void GrantPermission()
    {
        flagPermit = !flagPermit;


        modelSendInfo.GetComponent<SendInfoClient>().GrantPermissionClientRpc(flagPermit, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID },
            }
        });

        backPlateGrantPermissionButton.material = flagPermit ? materialRedButton : materialGreen;           

    }


    public void EnableHandButton()
    {
        handButton.GetComponent<Interactable>().IsEnabled = true;
        iconMeshRendererHand.material = materialYellow;
        backPlateHandButton.material = materialGreen;
    }

    public void RemoveNotification(bool flagSend = true)
    {
        handButton.GetComponent<Interactable>().IsEnabled = false;

        if (flagSend)
        {
            NetworkManager.Singleton.GetComponent<StartLesson>().ModifyUserArm(clientID, false);
            modelSendInfo.GetComponent<SendInfoClient>().RemoveNotificationClientRpc(new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientID },
                }
            });
        }

        iconMeshRendererHand.material = materialWhite;
        backPlateHandButton.material = materialGrey;
    }

    public void EnableDisableClientAction(bool flagSend)
    {
        bool enable = !handButton.activeSelf;
        handButton.SetActive(enable);

        if (enable)
            iconMeshRendererBlock.material = materialRed;

        if (flagSend)
        {
            NetworkManager.Singleton.GetComponent<StartLesson>().ModifyUserBlock(clientID);
            modelSendInfo.GetComponent<SendInfoClient>().EnableDisableClientRpc(enable, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientID },
                }
            });
        }
    }
}