using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinLessonHololens : MonoBehaviour
{
    [SerializeField] private GameObject canvasHol;
    [SerializeField] private GameObject codeObj;
    [SerializeField] private GameObject errorImage;
    [SerializeField] private GameObject placeholder;
    [SerializeField] private GameObject namePlayer;
    [SerializeField] private GameObject studentPrefab;

    async public void Join()
    {
        GameObject playerPrefab = studentPrefab;
        Debug.Log(namePlayer.GetComponent<MRTKTMPInputField>().text);
        playerPrefab.GetComponent<ClientHandler>().playerName = namePlayer.GetComponent<MRTKTMPInputField>().text;

        bool connectionOK = await NetworkManager.Singleton.GetComponent<RelayLogic>()
            .JoinRelay(codeObj.GetComponent<MRTKTMPInputField>().text);

        if (connectionOK)
        {
            Destroy(canvasHol.gameObject);
        }
        else
        {
            errorImage.SetActive(true);
            placeholder.GetComponent<TextMeshProUGUI>().text = "Codice errato";
            codeObj.GetComponent<MRTKTMPInputField>().text = "";
        }
    }
}