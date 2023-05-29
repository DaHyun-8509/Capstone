using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CropPicker : MonoBehaviour
{
    private GameObject uiText; //줍기 텍스트를 보여줄 오브젝트
    private GameObject currentCrop = null; //현재 트리거에 잡힌 작물

    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    void Update() //매 프레임마다
    {
        //현재 트리거에 잡힌 작물 있는 상태에서 E키 누를 때
        if (currentCrop != null && Input.GetKeyDown(KeyCode.E))
        {
            //텍스트 비활성화
            uiText.gameObject.SetActive(false);

            //작물 수확
            HarvestCrop();
        }
    }

    private void OnTriggerEnter(Collider other) //트리거 잡힐 때
    {

        if (other.CompareTag("PickableCropTrigger") && currentCrop == null)
        {
            //현재 트리거에 잡힌 작물이 없으면 지금 잡힌 작물을 저장
            currentCrop = other.gameObject.transform.parent.gameObject;

            //텍스트 활성화
            uiText.GetComponent<TextMeshProUGUI>().text = "줍기[E]";
            uiText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) //트리거에서 빠져나올 때
    {
        if (other.CompareTag("PickableCropTrigger"))
        {
            //텍스트 비활성화
            uiText.gameObject.SetActive(false);

            //현재 잡힌 작물 null로 초기화
            currentCrop = null;
        }
    }

    private void HarvestCrop() //작물 수확
    {
        if (currentCrop != null)
        {
            //작물 비활성화
            currentCrop.GetComponent<PickableCrop>().ClearCrop();

            //현재 작물 null 초기화
            currentCrop = null;
        }
    }
}
