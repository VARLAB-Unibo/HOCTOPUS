using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

//Script that controls and manages the active parts of the component
public class CheckActiveParts : MonoBehaviour
{   
    GameObject model;   //model in scene: generic Partitioning: Model --> Layers --> Containers --> Components
    private Dictionary<string, List<GameObject>> activePartsList = new(); //Dictionary with the active parts of each layer
    [SerializeField] private GameObject containers;             //Container buttons
    [SerializeField] private GameObject containerPartsPrefab;   //Prefab container of all components
    [SerializeField] private GameObject rootPartPrefab;         //Prefab container of a layer's "containers"
    [SerializeField] private GameObject buttonSpecificPartPrefab;//Prefab button

    private bool permissions = false;

    void Start()
    {   
        //Research model in the scene
        model = GameObject.FindGameObjectWithTag("SpawnedModel");
    }

    //Update of items to be selected
    public void UpdateSelectPanelParts()
    {
        if (permissions)
        {
            //Active part search and button instantiation
            activePartsList = GetActiveParts();
            DestroyAllChildren();
            AddNewChildren();
        }
    }

    //Creation of the dictionary with active parts
    private Dictionary<string, List<GameObject>> GetActiveParts()
    {
        Dictionary<string, List<GameObject>> dictParts = new Dictionary<string, List<GameObject>>();
        
        //iterations on the children of the model (the layers)
        foreach (Transform child in model.transform)
        {
            if (child.gameObject.activeSelf && child.tag == "Layer")
            {
                //Searching the active containers of the layer
                foreach (Transform nephew in child)
                {
                    if (nephew.gameObject.activeSelf && nephew.tag == "Container")
                    {
                        //If active container, we add it to the list of elements in its layer

                        if (!dictParts.ContainsKey(child.name))
                            dictParts.Add(child.name, new List<GameObject>());

                        dictParts[child.name].Add(nephew.gameObject);
                    }
                }
            }
        }

        return dictParts;
    }

    //destruction of all instantiated menu items
    private void DestroyAllChildren()
    {
        foreach (Transform container in containers.transform)
        {
            Destroy(container.gameObject);
        }
    }

    //Creation of active component buttons.
    private void AddNewChildren()
    {
        foreach (string nameLayer in activePartsList.Keys)
        {
            //For each active layer its Canvas container is created, which will be the parent of each block created for each active container.
            GameObject container = Instantiate(containerPartsPrefab, containers.transform);
            container.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = nameLayer;
            Transform containerButtons = container.transform.GetChild(1).transform;

            //Container canvas creation for each active container
            foreach (GameObject c in activePartsList[nameLayer])
            {
                GameObject newRoot = Instantiate(rootPartPrefab, containerButtons);
                newRoot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = c.name;
                //Creation buttons of the active parts of this container
                AddButtonsToRoot(newRoot, c);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(containers.GetComponent<RectTransform>());
    }

    //Creating buttons of active parts given a container
    private void AddButtonsToRoot(GameObject newRoot, GameObject rootPart)
    {
        //A button is instantiated for each active part
        foreach (Transform specificPart in rootPart.transform)
        {
            GameObject button = Instantiate(buttonSpecificPartPrefab.gameObject, newRoot.transform.GetChild(1));
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = specificPart.name;
        }
    }

    //Setting student component selection permissions
    public void SetPermission(bool permission)
    {
        permissions = permission;

        if (permissions)
            UpdateSelectPanelParts();
    }
}