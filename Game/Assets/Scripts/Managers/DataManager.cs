using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
    private List<Data> list = new List<Data>();

    public void Start()
    {
        LoadData<FoodData>("JsonData/item_food");
    }

    public void LoadData<T>(string path) where T : Data
    {
        var json = ReadFile(path).text;

        list.Add(JsonUtility.FromJson<T>(json));
    }

    private TextAsset ReadFile(string path)
    {
        var json = Resources.Load<TextAsset>(path);
        return json;
    }
    public IEnumerable<T> GetDatas<T>()
    {
        return this.list.OfType<T>();
    }

    public T GetData<T>(string id) where T : Data
    {
        var data = this.list.FindAll(x => x != null && x.GetType().Equals(typeof(T)));

        foreach (T dataBase in data)
        {
            foreach(var item in dataBase.info)
            {
                if (item.id == id)
                {
                    return (T)dataBase;
                }
            }
        }
        return null;
    }
}
