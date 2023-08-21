using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class StartLesson : MonoBehaviour
{
    [SerializeField] public Transform mixedRealitySC; //Parent where we spawn the objects to manipulate and share
    [SerializeField] public GameObject hololensCanvas; //Canvas where we display the lobby code
    [SerializeField] private TextMeshProUGUI lobbyCodeText; //LobbyCode we get from Relay
    [SerializeField] private Camera hololensCamera;

    [SerializeField] private Transform handMenuInfo;

    //ClientID --> Nome, Ha alzato la mano, Non può più alzare la mano (bloccato)
    private Dictionary<ulong, Tuple<string, bool, bool>> studentList =
        new Dictionary<ulong, Tuple<string, bool, bool>>();

    // Function where we spawn the object that corresponds to the selected card 
    public async void CreateClass(Transform modelToSpawn, Transform loadingBalls, Vector3 position,
        GameObject floorFinder)
    {
        //We activate the canvas where we display the lobby code
        hololensCanvas.gameObject.SetActive(true);

        //Getting the lobbycode
        string joinCode = await NetworkManager.Singleton.GetComponent<RelayLogic>().CreateRelay();

        //If the connection is ok we spawn the interested model
        if (joinCode != null)
        {
            Destroy(floorFinder);
            Destroy(loadingBalls.gameObject);
            SpawnObject(modelToSpawn, position);
            lobbyCodeText.text = "Code: " + joinCode;
            Transform codeText = FindDeepChild(handMenuInfo, "Code");
            codeText.GetComponent<TextMeshPro>().text = "CODE: " + joinCode;
            Instantiate(handMenuInfo);
        }
        else
        {
            lobbyCodeText.text = "Connection error! Join code NULL!";
        }
    }


    //Spawns the object in front of the hololens camera position
    public void SpawnObject(Transform model, Vector3 pos)
    {
        Vector3 relativePos = Camera.main.transform.position - pos;
        relativePos.y = 0;
        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //We spawn the obj in the scene, but we need to spawn it also for the network
        Transform spawnedModel = Instantiate(model, pos + new Vector3(0, 0.988f, 0), rotation, mixedRealitySC);
        spawnedModel.GetComponent<NetworkObject>().Spawn(true);
    }

    public void AddUser(ulong clientID, string studentName)
    {
        studentList.Add(clientID, new Tuple<string, bool, bool>(studentName, false, false));

        GameObject studentListObj = GameObject.FindGameObjectWithTag("StudentList");
        if (studentListObj != null)
        {
            studentListObj.GetComponent<ManageStudentList>().UpdateStudentListSpecific(clientID, studentList[clientID]);
        }
    }

    public void RemoveUser(ulong clientID)
    {
        bool removedOK = studentList.Remove(clientID);

        GameObject studentListObj = GameObject.FindGameObjectWithTag("StudentList");
        if (studentListObj != null)
        {
            studentListObj.GetComponent<ManageStudentList>().RemoveStudentSpecific(clientID);
        }
    }

    public void ModifyUserArm(ulong clientID, bool armRaised)
    {
        string clientName = studentList[clientID].Item1;
        studentList[clientID] = Tuple.Create(clientName, armRaised, studentList[clientID].Item3);
        GameObject studentListObj = GameObject.FindGameObjectWithTag("StudentList");

        if (studentListObj != null)
        {
            studentListObj.GetComponent<ManageStudentList>().UpdateRaiseButton(clientID, armRaised);
        }

        if (armRaised)
        {
            this.GetComponent<ManageNotification>().AddNotification(clientName);
        }
    }

    public void ModifyUserBlock(ulong clientID)
    {
        studentList[clientID] = Tuple.Create(studentList[clientID].Item1, studentList[clientID].Item2,
            !studentList[clientID].Item3);
    }

    public Dictionary<ulong, Tuple<string, bool, bool>> GetStudentList()
    {
        return studentList;
    }

    public Transform FindDeepChild(Transform parent, string name)
    {
        Transform child = parent.Find(name);
        if (child != null)
            return child;

        foreach (Transform tr in parent)
        {
            child = FindDeepChild(tr, name);
            if (child != null)
                return child;
        }

        return null;
    }
}