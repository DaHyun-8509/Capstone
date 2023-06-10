using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Farmer : MonoBehaviour
{
    private GameObject currentCropField = null; //현재 트리거에 잡힌 작물 영역

    private bool isWaitingForSelecting = false;



    void Update() //매 프레임마다
    {

        //현재 트리거에 잡힌 작물 있고 작물이 자라는 중이 아니라면
        if (currentCropField != null && currentCropField.GetComponent<CropField>().State != CropField.FieldState.Growing)
        {
            //E키 누르면
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameObject crop = currentCropField;
                //텍스트 비활성화
                Managers.UI.DisableFarmingUI();

                if (crop != null)
                {
                    //플레이어 상태 변경
                    GetComponent<PlayerController>().State = PlayerController.PlayerState.Interact;

                   
                    //자란 상태면 작물 수확
                    if (currentCropField.GetComponent<CropField>().State == CropField.FieldState.Grown)
                    {
                        Animator anim = GetComponent<Animator>();
                        anim.SetTrigger("pull_plant");
                        StartCoroutine(HarvestCrop());
                    }
                    //작물이 없으면 심기
                    else if (currentCropField.GetComponent<CropField>().Crop == null)
                    {
                        //UI보여주기 
                        Managers.UI.EnableFarmingUI();

                        //작물 고르고 심기
                        StartCoroutine(WaitforSelecting());
                        GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
                    }
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other) //트리거 잡힐 때
    {
        if (other.CompareTag("CropTrigger") && currentCropField == null)
        {
            //현재 트리거에 잡힌 필드 저장
            currentCropField = other.transform.parent.gameObject;

            //작물이 자란 상태면
            if (currentCropField.GetComponent<CropField>().State == CropField.FieldState.Grown)
            {
                Managers.UI.SetInteractText("수확하기[E]");
                Managers.UI.EnableInteractText();
            }
            //작물이 없으면
            else if (currentCropField.GetComponent<CropField>().Crop == null)
            {
                Managers.UI.SetInteractText("심기[E]");
                Managers.UI.EnableInteractText();
            }
        }
    }

    private void OnTriggerExit(Collider other) //트리거에서 빠져나올 때
    {
        if (other.CompareTag("CropTrigger"))
        {
            //텍스트 비활성화
            Managers.UI.DisableInteractText();

            //현재 잡힌 작물 null로 초기화
            currentCropField = null;
        }
    }

    private IEnumerator PlantCrop(CropField.CropType type)
    {
        yield return new WaitForSeconds(1);
        currentCropField.GetComponent<CropField>().Plant(type);

        GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
    }

    private IEnumerator HarvestCrop() //작물 수확
    {
        //작물이 수확 가능한 상태일 때
        if(currentCropField != null && currentCropField.GetComponent<CropField>().State == CropField.FieldState.Grown)
        {
            GameObject crop = currentCropField.GetComponent<CropField>().Crop;
            yield return new WaitForSeconds(1f);

            if (crop != null)
            {
                //기존 작물 삭제
                GameObject.Destroy(crop);
                currentCropField.GetComponent<CropField>().State = CropField.FieldState.Empty;

                //현재 작물 null 초기화
                currentCropField = null;
            }

            GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
        }
    }

    private IEnumerator WaitforSelecting()
    {
        isWaitingForSelecting = true;

        while(isWaitingForSelecting)
        {
            //입력 있을 때까지 대기
            yield return null;

            CropField.CropType type = CropField.CropType.None;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                type = CropField.CropType.Carrot;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                type = CropField.CropType.Corn;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                type = CropField.CropType.Cabbage;
            else if (Input.GetKeyDown(KeyCode.Escape)) //ESC누르면 종료
                isWaitingForSelecting = false;

            //작물 선택되었으면
            if(type != CropField.CropType.None)
            {
                Animator anim = GetComponent<Animator>();
                anim.SetTrigger("pull_plant");
                StartCoroutine(PlantCrop(type));
                isWaitingForSelecting = false;
            }
        }
        Managers.UI.DisableInteractText();

    }
}
