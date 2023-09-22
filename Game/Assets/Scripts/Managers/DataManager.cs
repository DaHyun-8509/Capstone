using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager
{
    private List<Food> foodList = new List<Food>();
    public List<Food> FoodList { get { return foodList; } }

    private List<Crop> cropList = new List<Crop>();
    public List<Crop> CropList { get { return cropList; } }


    public void Start()
    {
        //FoodData Load
        {
            var json = ReadFile("JsonData/item_food").text;
            FoodData foodData = JsonUtility.FromJson<FoodData>(json);
            foreach(var food in foodData.info) { foodList.Add(food); }
        }

    }

    private TextAsset ReadFile(string path)
    {
        var json = Resources.Load<TextAsset>(path);
        return json;
    }


    public Food GetFoodData(string id)
    {
        foreach(var food in foodList)
        {
            if (food.id == id)
                return food;
        }
        return null;
    }

    public Crop GetCropData(string id)
    {
        foreach (var crop in cropList)
        {
            if (crop.id == id)
                return crop;
        }
        return null;
    }

    public ItemBase GetItemData(string id)
    {
        {
            ItemBase item = GetFoodData(id);
            if (item != null) return item;
        }

        {
            ItemBase item = GetCropData(id);
            if (item != null) return item;
        }

        return null;
    }

}
