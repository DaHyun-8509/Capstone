using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MarketInventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool activateInventory = false;
    public GameObject player;

    //slot
    UI_MarketInvenSlot[] slots;
    public Transform slotHolder;

    UI_MarketInvenSlot selectedSlot;
    public UI_MarketInvenSlot SelectedSlot { get { return selectedSlot; } set { selectedSlot = value; } }

    [SerializeField]
    GameObject slotInfo;
    public GameObject SlotInfo { get { return slotInfo; } }

    public TextMeshProUGUI itemInfo_name;
    public TextMeshProUGUI itemInfo_price;
    public TextMeshProUGUI itemInfo_energy;

    private void Start()
    {
        slots = slotHolder.GetComponentsInChildren<UI_MarketInvenSlot>(); // content안의 slot 전부 갖고올수 있는 것
        Inventory.instance.onSlotCountChange += SlotChange;
        inventoryPanel.SetActive(activateInventory);
        Inventory.instance.onChangeItem += InventoryChanged;

        slotInfo.SetActive(false);
    }

    private void InventoryChanged()
    {
        //인벤토리 정보를 가져온다
        LinkedList<string> itemList;
        Dictionary<string, int> itemCountDict;
        Inventory.instance.GetInventoryItems(out itemList, out itemCountDict);

        int slotNum = 0;
        foreach (string itemId in itemList)
        {
            //각 슬롯을 인벤토리 리스트로 갱신
            slots[slotNum].Set(itemId, itemCountDict[itemId]);
            slotNum++;
        }
        for (int slot = slotNum; slot < slots.Length; slot++)
        {
            slots[slot].SetNull();
        }
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.instance.SlotCount)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void AddSlot()
    {
        Inventory.instance.SlotCount++;
    }

    public void AddItem(string item)
    {
        Inventory.instance.AddItem(item);
    }

}
