using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    crops,
    groceries,
    foods,
    etc
}


[System.Serializable]
public class Item 
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public bool Use()
    {
        return false; //������ ��� �������� ��ȯ
    }
}
