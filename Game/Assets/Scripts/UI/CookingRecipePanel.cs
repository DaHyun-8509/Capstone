using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookingRecipePanel : MonoBehaviour
{
    [SerializeField]
    UI_CookingSlot food;

    [SerializeField]
    UI_CookingSlot[] ingredients;

    [SerializeField]
    string foodId;
    [SerializeField]
    string[] ingredientIds;

    [SerializeField]
    Image checkImage;

    private void Start()
    {
        food.Set(foodId);

        food.ClearDark();
        int i = 0;
        for(;i<ingredientIds.Length;i++)
        {
            ingredients[i].Set(ingredientIds[i]);
        }
        for(; i <3;i++)
        {
            ingredients[i].SetNull();
        }

        UpdatePanel();
        Inventory.instance.onChangeItem += UpdatePanel;
    }

    public void UpdatePanel()
    {
        LinkedList<string> itemList;
        Dictionary<string, int> itemCountDict;
        Inventory.instance.GetInventoryItems(out itemList, out itemCountDict);

        checkImage.gameObject.SetActive(true);

        int i = 0;
        for (; i < ingredientIds.Length; i++)
        {
            //�������� ������
            if(itemList.Find(ingredientIds[i]) == null)
            {
                ingredients[i].AddDark();
                checkImage.gameObject.SetActive(false);
            }
            else //������
            {
                ingredients[i].ClearDark();
            }
        }
    }
}
