using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemBase : RawData
{
    public string name;
    public string description;
    public int purchase_price;
    public int sell_price;
}

[Serializable]
public class Food : ItemBase
{
    public int food_grade;
    public int energy;
}

[Serializable]
public class Crop : ItemBase
{

}

[Serializable]
public class Data
{
    
}

[Serializable]
public class FoodData : Data
{
    public  Food[] info;
}