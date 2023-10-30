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
    public int energy;
}

[Serializable]
public class Food : ItemBase
{
}

[Serializable]
public class Crop : ItemBase
{
    public float generate_time;
}

[Serializable]
public class Grocery : ItemBase
{

}

[Serializable]
public class Drink : ItemBase
{
    public int speed;
}



[Serializable]
public class FoodData
{
    public  Food[] info;
}

[Serializable]
public class CookFoodData
{
    public Food[] info;
}

[Serializable]
public class GroceryData
{
    public Grocery[] info;
}


[Serializable]
public class CropData
{
    public Crop[] info;
}

[Serializable]
public class DrinkData
{
    public Drink[] info;
}