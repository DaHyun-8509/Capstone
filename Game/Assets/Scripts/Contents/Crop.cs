using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private bool isHarvestable = true;
    public bool IsHarvestable
    {
        get { return isHarvestable; }
        set { isHarvestable = value; }
    }

}
