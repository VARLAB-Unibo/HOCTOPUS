using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateLabels : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] GameObject containerTooltips;

    private List<Transform> childsLeft;
    private List<Transform> childsRight;

    private List<Transform> objlabelleft;
    private List<Transform> objlabelright;
    [SerializeField] public bool recursive = false;
    [SerializeField] private Transform label;
    private float distance = 0.5f;
    private float step;
    private float initY;


    void Start()
    {
        Labelling();
    }

    //Function to instantiate the labels (tooltips) of the 3d model
    public void Labelling()
    {
        List<Transform> childrens = GetChildrens(this.transform);

        LeftRightChild(childrens);

        //Sorting of children according to their y position in the scene
        //childrens.Sort(YPositionComparison);
        childsRight.Sort(YPositionComparison);
        childsLeft.Sort(YPositionComparison);

        //Number of maximum labels placed on both the left and right sides of the model
        int nlabel = (int)System.Math.Ceiling(childrens.Count / 2.0f);

        //Value indicating how often to place a label
        step = 1.3f / nlabel;

        //Calculation of the initial position of the first label
        float yCenter = center.transform.position.y;
        initY = yCenter - (((int)System.Math.Ceiling(nlabel / 2.0f)) * step);

        objlabelleft = new List<Transform>();
        objlabelright = new List<Transform>();

        InstantiateLabels(childsRight, true);
        InstantiateLabels(childsLeft, false);
    }

    private void InstantiateLabels(List<Transform> listChilds, bool right)
    {
        foreach (Transform child in listChilds)
        {
            CreateLabel(child, right);
        }
    }

    private void LeftRightChild(List<Transform> childrens)
    {
        childsLeft = new List<Transform>();
        childsRight = new List<Transform>();

        /*if (!recursive)
        {*/
        foreach (Transform child in childrens)
        {
            if (child.localPosition.x >= 0)
            {
                childsRight.Add(child);
            }
            else
            {
                childsLeft.Add(child);
            }
        }

        int dif = Mathf.Abs(childsRight.Count - childsLeft.Count);

        if (dif > 1)
        {
            List<Transform> auxMax = childsRight.Count > childsLeft.Count ? childsRight : childsLeft;
            List<Transform> auxMin = childsRight.Count < childsLeft.Count ? childsRight : childsLeft;
            FixChildsList(auxMax, auxMin, dif / 2);
        }

        childsRight.Sort(YPositionComparison);
        childsLeft.Sort(YPositionComparison);
        /*}
        else
        {
            childrens.Sort(YPositionComparison);

            int aux = 0;
            foreach (Transform child in childrens)
            {
                if (aux % 2 == 0)
                    childsRight.Add(child);
                else
                    childsLeft.Add(child);

                aux++;
            }
        }*/
    }

    private void FixChildsList(List<Transform> maxList, List<Transform> minList, int n)
    {
        var sortedChilds = maxList.OrderBy(t => Mathf.Abs(t.position.x));
        var selectedChilds = sortedChilds.Take(n);

        foreach (Transform t in selectedChilds)
        {
            maxList.Remove(t);
            minList.Add(t);
        }
    }

    //Instantiation of a label
    private void CreateLabel(Transform child, bool right)
    {
        Vector3 rl;

        // if to decide whether the label will be placed to the right or left
        if (right)
            rl = this.transform.right;
        else
            rl = -(this.transform.right);

        //Calculation of label position and rotation
        Vector3 pos = child.transform.position + rl * distance;
        Quaternion rot = label.transform.rotation;

        //We fix the position on the y-axis
        pos = CheckPosition(pos, right);

        //Let's install the label and assign data related to it
        Transform spawnedModel = Instantiate(label, pos, rot, containerTooltips.transform);
        ToolTip labeltext = spawnedModel.GetComponent<ToolTip>();
        labeltext.ToolTipText = child.name;
        Transform anchor = spawnedModel.GetChild(0);
        anchor.position = child.position;

        if (right)
            objlabelright.Add(spawnedModel);
        else
            objlabelleft.Add(spawnedModel);
    }

    //Function to fix the position in the y-axis of the object
    private Vector3 CheckPosition(Vector3 poslabel, bool right)
    {
        List<Transform> objlabel;

        if (right)
            objlabel = objlabelright;
        else
            objlabel = objlabelleft;

        //If there are already other labels, the position of the next
        //one depends on the y-axis of the last positioned + step
        if (objlabel.Count > 0)
        {
            List<float> yposition = new List<float>();

            foreach (Transform i in objlabel)
            {
                yposition.Add(i.position.y);
            }

            poslabel.y = yposition.Max() + step;
        }
        else //First label in the list, starts from the initial position calculated first
        {
            poslabel.y = initY;
        }

        return poslabel;
    }

    //Function to retrieve children's transforms
    private List<Transform> GetChildrens(Transform parent)
    {
        //Auxiliary list where children are stored
        List<Transform> aux = new List<Transform>();

        foreach (Transform child in parent)
        {
            if (!child.name.Equals("Tooltips") && !child.name.Equals("Useless"))
            {   
                if(child.tag != "Container")
                    aux.Add(child);

                //If we need to recursively enter children's children, recursive will be True
                if (recursive && child.childCount > 0)
                {
                    List<Transform> grandchilds = GetChildrens(child);
                    aux.AddRange(grandchilds);
                }
            }
        }

        return aux;
    }

    // Comparison method to compare the height of the elements to which we will assign labels/
    private int YPositionComparison(Transform a, Transform b)
    {
        //null check, I consider nulls to be less than non-null
        if (a == null) return (b == null) ? 0 : -1;
        if (b == null) return 1;

        var ya = a.transform.position.y;
        var yb = b.transform.position.y;
        return ya.CompareTo(yb); //here I use the default comparison of floats
    }
}