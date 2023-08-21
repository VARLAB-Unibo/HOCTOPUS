using UnityEngine;

//Used for changing menu
public class ChangeMenu : MonoBehaviour
{
    public void GoToMenu(GameObject menuToSpawn, GameObject menuToDestroy, bool isToDestroy = true)
    {
        //Root where we want to spawn our menus
        GameObject root = GameObject.Find("UIHololens");
        Transform rootTransform = root != null ? root.transform : null;

        //We spawn the menu in our root
        GameObject menu = Instantiate(menuToSpawn, rootTransform);

        //We need to rotate the menu in such a way that it looks at us
        Transform cameraTransform = Camera.main.transform;
        menu.transform.position = cameraTransform.position + cameraTransform.forward / 2;
        menu.transform.LookAt(cameraTransform);
        menu.transform.RotateAround(menu.transform.position, menu.transform.up, 180f);

        //We destroy the menu that called this function
        if (isToDestroy)
            Destroy(menuToDestroy);
    }

    public void GoToMenuFromButton(GameObject menuToSpawn)
    {
        GoToMenu(menuToSpawn, this.gameObject);
    }
}