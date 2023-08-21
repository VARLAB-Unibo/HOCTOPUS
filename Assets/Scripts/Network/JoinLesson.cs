using TMPro;
using Unity.Netcode;
using UnityEngine;

public class JoinLesson : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_InputField code;
    [SerializeField] private GameObject errorImage;
    [SerializeField] private TextMeshProUGUI placeholder;
    [SerializeField] private TMP_InputField namePlayer;
    [SerializeField] private GameObject studentPrefab;

    async public void Join()
    {
        GameObject playerPrefab = studentPrefab;
        playerPrefab.GetComponent<ClientHandler>().playerName = namePlayer.text;

        bool connectionOK = await NetworkManager.Singleton.GetComponent<RelayLogic>().JoinRelay(code.text);

        if (connectionOK)
        {
            canvas.gameObject.SetActive(false);
        }
        else
        {
            errorImage.SetActive(true);
            placeholder.text = "Codice errato";
            code.text = "";
        }
    }
}