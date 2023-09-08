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
        //골드 체크 및 차감
        UpgradeButton(0);
    }
    public void UpgradeTo2()
    {
        //골드 체크 및 차감
        UpgradeButton(1);
    }
    public void UpgradeTo3()
    {
        //골드 체크 및 차감
        UpgradeButton(2);
    }
    public void UpgradeTo4()
    {
        //골드 체크 및 차감
        UpgradeButton(3);
    }

    private void UpgradeButton(int now)
    {
        interiors[now].SetActive(true);
        buttons[now].GetComponent<ButtonAction>().DeactiveButton();
        buttons[now].GetComponentInChildren<TextMeshProUGUI>().text = "구매완료";

        if (now < buttons.Length)
            buttons[now + 1].GetComponent<ButtonAction>().ActiveButton();
    }


}
