using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ResourceManager
{

    public Sprite GetSprite(string id)
    {
        return Resources.Load<Sprite>("JsonData/Sprites/" + id);
    }

}

