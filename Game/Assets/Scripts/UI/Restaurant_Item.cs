using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Restaurant_Item : MonoBehaviour
{
    Food food;
    
    public TextMeshProUGUI description_name;
    public TextMeshProUGUI description_price;
    public TextMeshProUGUI description_energy;
    public TextMeshProUGUI description_desc;
    public Image description_image;

    bool clicked = false;
    float clickTime = 0.0f;

    public void Init(Food item)
    {
        food = item;
        description_name.SetText(food.name);
        description_price.SetText("{0}GOLD", food.purchase_price);
        description_energy.SetText("에너지 +{0}", food.energy);
        //description_desc.SetText(food.description);
        description_image.sprite = Resources.Load<Sprite>("JsonData/" + food.id);
        
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
            Managers.Sound.PlayEating();
            Inventory.instance.AddItem(food.id);
        }
        else
        {
            //구매 불가능 (돈 부족) 
            
        }
    }
}
