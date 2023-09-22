using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    bool activateInventory = false;
    public GameObject player;

    UI_InvenSlot[] slots;
    public Transform slotHolder;


    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<UI_InvenSlot>(); // content안의 slot 전부 갖고올수 있는 것
        inven.onSlotCountChange += SlotChange;
        inventoryPanel.SetActive(activateInventory);
        inven.onChangeItem += InventoryChanged;
    }

    private void InventoryChanged()
    {
        //인벤토리 정보를 가져온다
        LinkedList<string> itemList;
        Dictionary<string, int> itemCountDict;
        inven.GetInventoryItems(out itemList, out itemCountDict);

        int slotNum = 0;
        foreach(string itemId in itemList)
        {
            //각 슬롯을 인벤토리 리스트로 갱신
            slots[slotNum].Set(itemId, itemCountDict[itemId]);
            slotNum++;
        }
        for(int slot = slotNum; slot < slots.Length; slot++)
        {
            slots[slot].SetNull();
        }
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++) {
            if (i < inven.SlotCount)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activateInventory = !activateInventory;
            inventoryPanel.SetActive(activateInventory);

            if(activateInventory)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                player.GetComponent<PlayerController>().State = PlayerController.PlayerState.Interact;
                player.GetComponent<CharacterController>().enabled = false;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                player.GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
                player.GetComponent<CharacterController>().enabled = true;
            }
        }
    }

public void AddSlot()
    {
        inven.SlotCount++;
    }

    public void AddItem(string item)
    {
        inven.AddItem(item);
    }
}
