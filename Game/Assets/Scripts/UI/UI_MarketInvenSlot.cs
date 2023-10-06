using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MarketInvenSlot : MonoBehaviour
{
    UI_MarketInventory inventoryUI;

    ItemBase item;
    int itemCount = 0;
    public TextMeshProUGUI item_count;
    public Image item_image;

    bool mouseOn = false;
    public bool MouseOn { get { return mouseOn; } }

    GameObject item_slotInfo;

    [SerializeField]
    Transform infoPos;

    private void Start()
    {
        inventoryUI = GameObject.Find("Market_InventoryUI").GetComponent<UI_MarketInventory>();
        item_slotInfo = inventoryUI.SlotInfo;

    }

    public void Set(string itemId, int count)
    {
        //이미지

        item = Managers.Data.GetItemData(itemId);

        item_image.sprite = Resources.Load<Sprite>("JsonData/" + item.id);
        Color color = item_image.color;
        color.a = 1f;
        item_image.color = color;

        //개수
        itemCount = count;
        item_count.SetText(itemCount.ToString());
    }

    public void SetNull()
    {
        item = null;
        Color color = item_image.color;
        color.a = 0f;
        item_image.color = color;

        item_count.SetText("");
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if ((Food)item != null)
            {
                //TODO
                //아래 슬롯에 뜨게 하기
            }
        }
    }

    public void RemoveItem(int count)
    {
        Inventory.instance.RemoveItem(item.id, count);
    }

}
