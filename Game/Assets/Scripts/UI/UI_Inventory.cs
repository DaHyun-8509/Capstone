using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    bool activateInventory = false;


    public Slot[] slots;
    public Transform slotHolder;


    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>(); // content안의 slot 전부 갖고올수 있는 것
        inven.onSlotCountChange += SlotChange;
        inventoryPanel.SetActive(activateInventory);
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++) {
            if (i < inven.SlotCnt)
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
        }
    }

public void AddSlot()
    {
        inven.SlotCnt++;
    }

}
