using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemBase : RawData
{
    public string name;
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
public class Grocery : ItemBase 
{ 

}


[Serializable]
public class FoodData
{
    public  Food[] info;
}

[Serializable]
public class GroceryData
{
    public Grocery[] info;
}