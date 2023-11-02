using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Restaurant_Item : MonoBehaviour
{
    ItemBase item;
    public TextMeshProUGUI description_name;
    public TextMeshProUGUI description_price;
    public TextMeshProUGUI description_energy;
    public Image description_image;

    bool clicked = false;
    float clickTime = 0.0f;


    [SerializeField]
    TextMeshProUGUI guideTextUI;

    public void Init(Food food)
    {
        item = food;
        description_name.SetText(food.name);
        description_price.SetText("{0}GOLD", food.purchase_price);
        description_energy.SetText("������ +{0}", food.energy);
        description_image.sprite = Managers.Resource.GetSprite(food.id);
    }

    public void InitDrink(Drink drink)
    {
        item = drink;
        description_name.SetText(drink.name);
        description_price.SetText("{0}GOLD", drink.purchase_price);
        description_energy.SetText("������ {0}", drink.energy);
        description_image.sprite = Managers.Resource.GetSprite(drink.id);
    }

    public void Update()
    {
        clickTime += Time.unscaledDeltaTime;
        Debug.Log(clickTime);
    }

    public void OnButtonClicked()
    {
        //����Ŭ�� �Ǵ� 
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
        if(Managers.Gold.SubGold(item.purchase_price))
        {
            //���� �õ�
            if(Inventory.instance.AddItem(item.id, 1))
            { //���� ����

                guideTextUI.SetText("���� �Ϸ�!");
                guideTextUI.color = Color.green;
                StartCoroutine(WaitAndRemoveGuideText());
            }
            else
            {   //���� �Ұ��� (���� ����)

                guideTextUI.SetText("�κ��丮 ������ �����մϴ�!");
                guideTextUI.color = Color.red;
                StartCoroutine(WaitAndRemoveGuideText());
                //������ ��� �ٽ� ����
                Managers.Gold.AddGold(item.purchase_price);
            }
        }
        else
        {
            //���� �Ұ��� (�� ����) 
            guideTextUI.SetText("��尡 �����մϴ�!");
            guideTextUI.color = Color.red;
            StartCoroutine(WaitAndRemoveGuideText());
        }
    }
    IEnumerator WaitAndRemoveGuideText()
    {
        yield return new WaitForSecondsRealtime(1f);
        guideTextUI.SetText("");
    }
}
