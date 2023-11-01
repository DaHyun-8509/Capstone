using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CookBook : MonoBehaviour
{
    [SerializeField] GameObject[] recipes;
    [SerializeField] TextMeshProUGUI guideText;
    int nowIdx = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && nowIdx < recipes.Length)
        {
            Managers.UI.SetInteractText("[E] �丮�� ����(100���)");
            Managers.UI.EnableInteractText();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E) && nowIdx < recipes.Length)
            {
                if (Managers.Gold.GetGold() < 100)
                {
                    guideText.SetText("��尡 �����մϴ�!");
                }
                else
                {
                    Managers.Gold.SubGold(100);
                    recipes[nowIdx].gameObject.SetActive(true);
                    guideText.SetText("�丮���� �����Ͽ����ϴ� : "+ Managers.Data.GetItemData(recipes[nowIdx].GetComponent<CookingRecipePanel>().foodId).name);
                    nowIdx++;
                }
                guideText.gameObject.SetActive(true);
                StartCoroutine(HideGuideMessage());
            }

        }
    }

    IEnumerator HideGuideMessage()
    {
        yield return new WaitForSeconds(0.5f);
        if (nowIdx >= recipes.Length)
        {
            guideText.SetText("�丮���� ��� �����Ͽ����ϴ�.");
        }
        yield return new WaitForSeconds(0.5f);
        guideText.gameObject.SetActive(false);
    }
}
