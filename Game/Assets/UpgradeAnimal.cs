using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UpgradeAnimal : MonoBehaviour
{
    [SerializeField]
    GameObject basicCage;
    [SerializeField]
    GameObject extendedCage;
    [SerializeField]
    GameObject chicken;

    [SerializeField]
    GameObject basicCageButton;

    [SerializeField]
    GameObject extendedCageButton;

    [SerializeField]
    GameObject getChickenButton;

    [SerializeField]
    Transform chickenSpawnPos;

    enum CageState
    {
        None, Basic, Extended
    }

    CageState state = CageState.None;

    [SerializeField]
    int basicChickenCount;

    [SerializeField]
    int extendedChickenCount;

    int chickenCount = 0;

    private void Start()
    {
        extendedCageButton.GetComponent<ButtonAction>().DeactiveButton();    
        getChickenButton.GetComponent<ButtonAction>().DeactiveButton();
    }
    public void BuyBasicCage()
    {
        //골드 체크 및 차감
        basicCage.SetActive(true);
        state = CageState.Basic;

        basicCageButton.GetComponent<ButtonAction>().DeactiveButton();
        basicCageButton.GetComponentInChildren<TextMeshProUGUI>().text = "구매완료";
        extendedCageButton.GetComponent<ButtonAction>().ActiveButton();
        getChickenButton.GetComponent<ButtonAction>().ActiveButton();
    }
    public void BuyExtendedCage()
    {
        //골드 체크 및 차감
        basicCage.SetActive(false);
        extendedCage.SetActive(true);
        state = CageState.Extended;

        extendedCageButton.GetComponent<ButtonAction>().DeactiveButton();
        extendedCageButton.GetComponentInChildren<TextMeshProUGUI>().text = "구매완료";

        getChickenButton.GetComponent<ButtonAction>().ActiveButton();
        getChickenButton.GetComponentInChildren<TextMeshProUGUI>().text = "구매";
    }
    public void BuyChicken()
    {
        //골드 체크 및 차감
        
        if((state == CageState.Basic && chickenCount < basicChickenCount)
            ||(state == CageState.Extended && chickenCount < extendedChickenCount)) 
        {
            
            GameObject newChicken = Instantiate(chicken, chickenSpawnPos);
            newChicken.transform.parent = chickenSpawnPos;

            chickenCount++;
        }

        if((state == CageState.Basic && chickenCount >= basicChickenCount)
            || (state == CageState.Extended && chickenCount >= extendedChickenCount))
        {
            getChickenButton.GetComponent<ButtonAction>().DeactiveButton();
            getChickenButton.GetComponentInChildren<TextMeshProUGUI>().text = "최대";
        }
    }

}
