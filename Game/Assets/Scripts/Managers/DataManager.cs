using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
    private List<RawData> dataList = new List<RawData>();

    public void Start()
    {
        LoadData<JsonCrop>("JsonData/item_crop");
        LoadData<JsonFood>("JsonData/item_food");
    }

    private void LoadData<T>(string path) where T: RawData
    {
        var json = this.ReadFile(path).text;
        var arr = JsonConvert.DeserializeObject<T[]>(json);

        if (arr != null)
            this.dataList.AddRange(arr);
    }

    private TextAsset ReadFile(string path)
    {
        var json = Resources.Load<TextAsset>(path);
        return json;
    }

    public IEnumerable<T> GetDatas<T>()
    {
        return this.dataList.OfType<T>();
    }

    public T GetData<T>(string id) where T: RawData
    {
        var data = this.dataList.FindAll(x => x != null && x.GetType().Equals(typeof(T)));

        foreach(RawData rawData in data)
        {
            if (rawData.id == id)
                return (T)rawData;
        }
        return null;
    }
}
