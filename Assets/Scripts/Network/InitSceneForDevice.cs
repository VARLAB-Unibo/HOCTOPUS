using UnityEngine;

public class InitSceneForDevice : MonoBehaviour
{
    [SerializeField] private GameObject arSession;
    [SerializeField] private GameObject startMenuHol;
    [SerializeField] private GameObject canvasAnd;
    [SerializeField] private GameObject hololensStuffs;
    [SerializeField] private GameObject hololensToolkit;


    void Start()
    {
        bool isAndroid = Application.platform == RuntimePlatform.Android;

        if (!isAndroid)
        {
            SpawnStartMenu(startMenuHol, true);
        }
        else
        {
            SpawnStartMenu(canvasAnd, false);
        }

        arSession.gameObject.SetActive(isAndroid);
        hololensStuffs.gameObject.SetActive(!isAndroid);
        hololensToolkit.gameObject.SetActive(!isAndroid);
    }

    private void SpawnStartMenu(GameObject menuToSpawn, bool flag)
    {
        Transform parentTransform;

        if (flag)
        {
            parentTransform = FindParentTransform("UIHololens");
        }
        else
        {
            parentTransform = FindParentTransform("Canvas");
        }

        GameObject menu = Instantiate(menuToSpawn, parentTransform);

        if (flag)
        {
            Transform tranCam = Camera.main.transform;
            menu.transform.position = tranCam.position + tranCam.forward / 2;
            menu.transform.LookAt(tranCam);
            menu.transform.RotateAround(menu.transform.position, menu.transform.up, 180f);
        }
    }

    private Transform FindParentTransform(string name)
    {
        return GameObject.Find(name).transform;
    }
}