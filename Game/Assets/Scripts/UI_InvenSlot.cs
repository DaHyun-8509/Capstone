using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UI_Inventory inventoryUI;

    ItemBase item;
    int itemCount = 0;
    public TextMeshProUGUI item_count;
    public Image item_image;
    
    GameObject item_slotInfo;

    [SerializeField]
    Transform infoPos;

    private void Start()
    {
        inventoryUI = GameObject.Find("Inventory").GetComponent<UI_Inventory>();
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
        Color color = item_image.color;
        color.a = 0f;
        item_image.color = color;

        item_count.SetText("");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        // info 위치 조정
        RectTransform infoUIPos = item_slotInfo.GetComponent<RectTransform>();
        Vector3 localPosition = inventoryUI.transform.InverseTransformPoint(infoPos.position);
        infoUIPos.localPosition = localPosition;
        item_slotInfo.SetActive(true);

        // info 텍스트 변경
        inventoryUI.itemInfo_name.text = item.name;
        inventoryUI.itemInfo_price.text = item.sell_price + "GOLD";
        if ((Food)item != null)
            inventoryUI.itemInfo_energy.text = "에너지 +" + ((Food)item).energy;
        else
            inventoryUI.itemInfo_energy.text = "";
        
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        item_slotInfo.SetActive(false);
    }

}
