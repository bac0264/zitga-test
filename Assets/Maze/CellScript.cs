using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{

    public GameObject wallL;
    public GameObject wallR;
    public GameObject wallU;
    public GameObject wallD;

    public GameObject LineL;
    public GameObject LineR;
    public GameObject LineU;
    public GameObject LineD;
    int direct;
    public int DIRECT
    {
        set
        {
            direct = value;
            if (value == 0) LineL.SetActive(true); 
            else if (value == 1) LineR.SetActive(true);
            else if (value == 2) LineU.SetActive(true);
            else if (value == 3) LineD.SetActive(true);

            else
            {
                LineL.SetActive(false);
                LineR.SetActive(false);
                LineU.SetActive(false);
                LineD.SetActive(false);
            }
        }
        get
        {
            return direct;
        }
    }
    public bool LeftWall
    {
        set
        {
            wallL.SetActive(value);
        }
        get
        {
            if (wallL.activeInHierarchy) return true;
            return false;
        }
    }
    public bool RightWall
    {
        set
        {
            wallR.SetActive(value);
        }
        get
        {
            if (wallR.activeInHierarchy) return true;
            return false;
        }
    }
    public bool UpWall
    {
        set
        {
            wallU.SetActive(value);
        }
        get
        {
            if (wallU.activeInHierarchy) return true;
            return false;
        }
    }
    public bool DWall
    {
        set
        {
            wallD.SetActive(value);
        }
        get
        {
            if (wallD.activeInHierarchy) return true;
            return false;
        }
    }

}
