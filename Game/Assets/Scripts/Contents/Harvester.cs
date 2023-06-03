using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Harvester : MonoBehaviour
{
    private GameObject uiText; //수확하기 텍스트를 보여줄 오브젝트
    private GameObject currentCropField = null; //현재 트리거에 잡힌 작물 영역
    
    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    void Update() //매 프레임마다
    {
        //현재 트리거에 잡힌 작물 있는 상태에서 E키 누를 때
        if (currentCropField != null && Input.GetKeyDown(KeyCode.E))
        {
            //텍스트 비활성화
            uiText.gameObject.SetActive(false);

            //작물 수확
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("pull_plant");

            StartCoroutine(HarvestCrop());
        }
    }

    private void OnTriggerEnter(Collider other) //트리거 잡힐 때
    {
      
        if (other.CompareTag("CropTrigger")&& currentCropField == null)
        {
            //현재 트리거에 잡힌 작물이 없으면 지금 잡힌 작물을 저장
            currentCropField = other.transform.parent.gameObject;

            //텍스트 활성화
            uiText.GetComponent<TextMeshProUGUI>().text = "수확하기[E]";
            uiText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) //트리거에서 빠져나올 때
    {
        if (other.CompareTag("CropTrigger"))
        {
            //텍스트 비활성화
            uiText.gameObject.SetActive(false);

            //현재 잡힌 작물 null로 초기화
            currentCropField = null;
        }
    }

    private IEnumerator HarvestCrop() //작물 수확
    {
        //작물이 수확 가능한 상태일 때
        if(currentCropField != null && currentCropField.GetComponent<CropField>().IsGrown)
        {
            GameObject crop = currentCropField;
            yield return new WaitForSeconds(1f);

            if (crop != null)
            {
                //기존 작물 비활성화
                crop.SetActive(false);

                //일정 시간 후 자라게
                StartCoroutine(crop.GetComponent<CropField>().GrowCropAfterDelay());

                //현재 작물 null 초기화
                currentCropField = null;
            }
        }
    }
}
