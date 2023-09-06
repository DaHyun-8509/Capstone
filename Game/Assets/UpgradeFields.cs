using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeFields : MonoBehaviour
{
    [SerializeField]
    GameObject[] fields;
    [SerializeField]
    GameObject[] buttons;

    int fieldLevel = 0;

    private void Start()
    {
        for(int i = 1;i < buttons.Length; i++)
        {
            buttons[i].GetComponent<ButtonAction>().DeactiveButton();
        }
    }

    public void UpgradeTo1()
    {
        //��� üũ �� ����
        fields[0].SetActive(true);
        buttons[0].GetComponent<ButtonAction>().DeactiveButton();
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "���ſϷ�";
        buttons[1].GetComponent<ButtonAction>().ActiveButton();
    }
    public void UpgradeTo2()
    {
        //��� üũ �� ����
        fields[1].SetActive(true);
        buttons[1].GetComponent<ButtonAction>().DeactiveButton();
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "���ſϷ�";
        buttons[2].GetComponent<ButtonAction>().ActiveButton();
    }
    public void UpgradeTo3()
    {
        //��� üũ �� ����
        fields[2].SetActive(true);
        buttons[2].GetComponent<ButtonAction>().DeactiveButton();
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "���ſϷ�";
        buttons[3].GetComponent<ButtonAction>().ActiveButton();
    }
    public void UpgradeTo4()
    {
        //��� üũ �� ����
        fields[3].SetActive(true);
        buttons[3].GetComponent<ButtonAction>().DeactiveButton();
        buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = "���ſϷ�";
    }

}
