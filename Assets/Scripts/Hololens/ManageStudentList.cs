using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ManageStudentList : MonoBehaviour
{
    [SerializeField] private Transform studentListInMenu;
    [SerializeField] private TextMeshPro studentCounterLabel;
    [SerializeField] private Transform studentNamePrefab;
    private int studentCounter = 0;

    public void UpdateStudentList()
    {
        Dictionary<ulong, Tuple<string, bool, bool>> studentList = NetworkManager.Singleton.GetComponent<StartLesson>().GetStudentList();

        DeleteStudentList();

        foreach (var student in studentList)
        {
            UpdateStudentListSpecific(student.Key, student.Value);
        }
    }


    public void UpdateStudentListSpecific(ulong studentKey, Tuple<string, bool, bool> studentValue)
    {
        studentCounter++;
        studentCounterLabel.text = "Connected students: " + studentCounter;
        studentNamePrefab.GetComponent<TextMeshPro>().text = studentValue.Item1;
        Transform obj = Instantiate(studentNamePrefab, studentListInMenu);
        obj.GetComponent<StudentLabelHandler>().SetClientID(studentKey);
        if (studentValue.Item2) {
            obj.GetComponent<StudentLabelHandler>().EnableHandButton();
        }

        if (studentValue.Item3) {
            obj.GetComponent<StudentLabelHandler>().EnableDisableClientAction(false);
        }

        studentListInMenu.GetComponent<GridObjectCollection>().UpdateCollection();
    }


    public void DeleteStudentList()
    {
        studentCounter = 0;
        foreach (Transform child in studentListInMenu)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        Debug.Log("DeleteStudentList() " + studentListInMenu.childCount);
    }

    public void RemoveStudentSpecific(ulong clientID)
    {
        foreach (Transform student in studentListInMenu)
        {
            if (student.GetComponent<StudentLabelHandler>().GetClientID() == clientID)
            {
                Destroy(student.gameObject);
                studentCounter--;
                studentCounterLabel.text = "Studenti collegati: " + studentCounter;
            }
        }
        studentListInMenu.GetComponent<GridObjectCollection>().UpdateCollection();
    }

    public void UpdateRaiseButton(ulong clientID, bool armRaised) {
        foreach (Transform student in studentListInMenu) {            
            if (student.GetComponent<StudentLabelHandler>().GetClientID() == clientID) {
                if(armRaised)
                    student.GetComponent<StudentLabelHandler>().EnableHandButton();
                else
                    student.GetComponent<StudentLabelHandler>().RemoveNotification(false);
            }
        }
    }
}