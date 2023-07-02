using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class UIManager
{
    GameObject canvas;
    GameObject interactText;
    GameObject famringUI;
    GameObject timeText;

    public void Start()
    {
        {
            canvas = GameObject.Find("Canvas");
        }
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/UI_InteractionText");
            interactText = GameObject.Instantiate(prefab, canvas.transform);
            interactText.SetActive(false);
        }
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/UI_Farming");
            famringUI = GameObject.Instantiate(prefab, canvas.transform);
            famringUI.SetActive(false);
        }
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/UI_Time");
            timeText = GameObject.Instantiate(prefab, canvas.transform);
            timeText.SetActive(true);
        }
    }

    public void Update()
    {
        timeText.GetComponent<TextMeshProUGUI>().SetText(Managers.Time.GetHour().ToString() + "½Ã");
    }

    public void EnableCanvas()
    {
        canvas.SetActive(true);
    }
    public void DisableCanvas()
    {
        canvas.SetActive(false);

    }
    public void EnableInteractText()
    {
        interactText.SetActive(true);
    }
    public void DisableInteractText()
    {
        interactText.SetActive(false);

    }
    
    public void SetInteractText(string text)
    {
        interactText.GetComponent<TextMeshProUGUI>().text = text;
    }


    public void EnableFarmingUI()
    {
        famringUI.SetActive(true);
    }
    public void DisableFarmingUI()
    {
        famringUI.SetActive(false);

    }

}
