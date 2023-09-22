using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour
{


    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int val); // 대리자 정의
    public OnSlotCountChange onSlotCountChange;


    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    LinkedList<string> items = new LinkedList<string>();
    Dictionary</*id*/ string,/*count*/ int> itemCounts = new Dictionary<string, int>();

    
    private int slotCount;
    public int SlotCount
    {
        get => slotCount;
        set
        {
            slotCount = value;
            onSlotCountChange.Invoke(slotCount);
        }
    }

    void Start()
    {
        slotCount = 20;   

    }

    public void GetInventoryItems(out LinkedList<string> itemList, out Dictionary<string, int> itemCountDict)
    {
        itemList = items;
        itemCountDict = itemCounts;
    }

    public bool AddItem(string itemId)
    {
        if (items.Count < slotCount)
        {
            
            if(itemCounts.ContainsKey(itemId))
            {
                itemCounts[itemId]++;
            }
            else
            {
                itemCounts.Add(itemId, 1);
                items.AddLast(itemId);
            }

            onChangeItem.Invoke();
            return true;
        }
        return false;
    }

}
