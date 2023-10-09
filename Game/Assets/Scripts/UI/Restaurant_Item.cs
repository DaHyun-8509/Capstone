using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Restaurant_Item : MonoBehaviour
{
    Food food;
    
    public TextMeshProUGUI description_name;
    public TextMeshProUGUI description_price;
    public TextMeshProUGUI description_energy;
    public Image description_image;

    bool clicked = false;
    float clickTime = 0.0f;

    public void Init(Food item)
    {
        food = item;
        description_name.SetText(food.name);
        description_price.SetText("{0}GOLD", food.purchase_price);
        description_energy.SetText("에너지 +{0}", food.energy);
        description_image.sprite = Managers.Resource.GetSprite(item.id);

    }
    public void Update()
    {
        clickTime += Time.unscaledDeltaTime;
        Debug.Log(clickTime);
    }

    public void OnButtonClicked()
    {
        //더블클릭 판단 
        if (clicked == false)
        {
            clicked = true;
            clickTime = 0.0f;
        }
        else if (clickTime > 0.3f)
        {
            clicked = false;
            clickTime = 0.0f;
        }
        else
        {
            Purchase();
            clicked = false;
            clickTime = 0.0f;
        }
    }

    private void Purchase()
    {
        if(Managers.Gold.SubGold(food.purchase_price))
        {
            //구매 완료
            //소리
            //TODO
            Inventory.instance.AddItem(food.id, 1);
        }
        else
        {
            //구매 불가능 (돈 부족) 
            
        }
    }
}
