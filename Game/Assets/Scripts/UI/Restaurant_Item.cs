using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Restaurant_Item : MonoBehaviour
{
    Food food;
    
    public TextMeshProUGUI description_name;
    public TextMeshProUGUI description_price;
    public TextMeshProUGUI description_energy;
    public TextMeshProUGUI description_desc;

    public void Init(Food item)
    {
        food = item;
        description_name.SetText(food.name);
        description_price.SetText("{0}GOLD", food.purchase_price);
        description_energy.SetText("¿¡³ÊÁö +{0}", food.energy);
        //description_desc.SetText(food.description);
    }
}
