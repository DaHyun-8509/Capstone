using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeInterior : MonoBehaviour
{
    [SerializeField]
    GameObject[] interiors;

    [SerializeField]
    GameObject[] buttons;

    int interiorLevel = 0;

    private void Start()
    {
        for (int i = 1; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<ButtonAction>().DeactiveButton();
        }
    }
    public void UpgradeTo1()
    {
        //��� üũ �� ����
        UpgradeButton(0);
    }
    public void UpgradeTo2()
    {
        //��� üũ �� ����
        UpgradeButton(1);
    }
    public void UpgradeTo3()
    {
        //��� üũ �� ����
        UpgradeButton(2);
    }
    public void UpgradeTo4()
    {
        //��� üũ �� ����
        UpgradeButton(3);
    }

    private void UpgradeButton(int now)
    {
        interiors[now].SetActive(true);
        buttons[now].GetComponent<ButtonAction>().DeactiveButton();
        buttons[now].GetComponentInChildren<TextMeshProUGUI>().text = "���ſϷ�";

        if (now < buttons.Length)
            buttons[now + 1].GetComponent<ButtonAction>().ActiveButton();
    }


}
