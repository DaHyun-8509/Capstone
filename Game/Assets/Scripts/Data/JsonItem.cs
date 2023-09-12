using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonItem : RawData
{
    public string name;
    public string description;
    public int purchase_price;
    public int sell_price;
    public int type;
}

public class JsonCrop : JsonItem
{
    public int crop_grade;
}

public class JsonIngredient : JsonItem
{

}

public class JsonFood : JsonItem
{
    public int food_grade;
    public int energy;
}