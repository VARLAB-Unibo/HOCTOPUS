using Unity.Netcode;
using UnityEngine;

public class DisableHololensMainStuffFromServer : NetworkBehaviour
{
    [SerializeField] private GameObject handMenuToggles;
    [SerializeField] private GameObject menuDeepSkeleton;
    [SerializeField] private GameObject menuDeepOrgans;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            //Guardare se sono da distruggere
            handMenuToggles.SetActive(false);
        }
        else {

            Transform parentUIHololens = GameObject.Find("UIHololens").transform;
            handMenuToggles.transform.SetParent(parentUIHololens, true);
            
            if (menuDeepSkeleton != null || menuDeepOrgans != null)
            {
                menuDeepSkeleton.transform.SetParent(parentUIHololens, true);
                menuDeepOrgans.transform.SetParent(parentUIHololens, true);   
            }
        }
    }
}
