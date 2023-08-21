using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using TMPro;
using UnityEngine;

//Script for activating the outline upon clicking the button of the chosen component
public class ActiveOutlineFromClient : MonoBehaviour
{
    private string partName;
    private Transform root;


    void Start()
    {
        root = this.transform.root;
    }

    public void OutlineFromClient()
    {
        //Outline activation and button logic.
        partName = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        root.GetComponent<ClientHandler>().ShowOutline(partName);
        root.GetComponent<ClientHandler>().ChoosePartClick();
    }
}