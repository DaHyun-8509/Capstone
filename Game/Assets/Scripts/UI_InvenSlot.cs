using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InvenSlot : MonoBehaviour
{
    ItemBase item;
    int itemCount = 0;
    //public TextMeshProUGUI item_name;
    //public TextMeshProUGUI item_price;
    //public TextMeshProUGUI item_energy;
    //public TextMeshProUGUI item_desc;
    public TextMeshProUGUI item_count;
    public Image item_image;

    public void Set(string itemId, int count)
    {
        //이미지

        item = Managers.Data.GetItemData(itemId);

        item_image.sprite = Resources.Load<Sprite>("JsonData/" + item.id);

        //개수
        itemCount = count;
        item_count.SetText(itemCount.ToString());
    }
}
