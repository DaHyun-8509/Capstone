using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Farmer : MonoBehaviour
{
    private GameObject uiText; //��Ȯ�ϱ� �ؽ�Ʈ�� ������ ������Ʈ
    private GameObject farmingUI;
    private GameObject currentCropField = null; //���� Ʈ���ſ� ���� �۹� ����

    private bool isWaitingForSelecting = false;


    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
        farmingUI = GameObject.Find("UI_Farming");
        farmingUI.SetActive(false);
    }

    void Update() //�� �����Ӹ���
    {

        //���� Ʈ���ſ� ���� �۹� �ְ� �۹��� �ڶ�� ���� �ƴ϶��
        if (currentCropField != null && currentCropField.GetComponent<CropField>().State != CropField.FieldState.Growing)
        {
            //EŰ ������
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameObject crop = currentCropField;
                //�ؽ�Ʈ ��Ȱ��ȭ
                uiText.gameObject.SetActive(false);

                if (crop != null)
                {
                    //�÷��̾� ���� ����
                    GetComponent<PlayerController>().State = PlayerController.PlayerState.Interact;

                   
                    //�ڶ� ���¸� �۹� ��Ȯ
                    if (currentCropField.GetComponent<CropField>().State == CropField.FieldState.Grown)
                    {
                        Animator anim = GetComponent<Animator>();
                        anim.SetTrigger("pull_plant");
                        StartCoroutine(HarvestCrop());
                    }
                    //�۹��� ������ �ɱ�
                    else if (currentCropField.GetComponent<CropField>().Crop == null)
                    {
                        //UI�����ֱ� 
                        farmingUI.SetActive(true);

                        //�۹� ���� �ɱ�
                        StartCoroutine(WaitforSelecting());
                        GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
                    }
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other) //Ʈ���� ���� ��
    {
        if (other.CompareTag("CropTrigger") && currentCropField == null)
        {
            //���� Ʈ���ſ� ���� �ʵ� ����
            currentCropField = other.transform.parent.gameObject;

            //�۹��� �ڶ� ���¸�
            if (currentCropField.GetComponent<CropField>().State == CropField.FieldState.Grown)
            {
                uiText.GetComponent<TextMeshProUGUI>().text = "��Ȯ�ϱ�[E]";
                uiText.gameObject.SetActive(true);
            }
            //�۹��� ������
            else if (currentCropField.GetComponent<CropField>().Crop == null)
            {
                uiText.GetComponent<TextMeshProUGUI>().text = "�ɱ�[E]";
                uiText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) //Ʈ���ſ��� �������� ��
    {
        if (other.CompareTag("CropTrigger"))
        {
            //�ؽ�Ʈ ��Ȱ��ȭ
            uiText.gameObject.SetActive(false);

            //���� ���� �۹� null�� �ʱ�ȭ
            currentCropField = null;
        }
    }

    private IEnumerator PlantCrop(CropField.CropType type)
    {
        yield return new WaitForSeconds(1);
        currentCropField.GetComponent<CropField>().Plant(type);

        GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
    }

    private IEnumerator HarvestCrop() //�۹� ��Ȯ
    {
        //�۹��� ��Ȯ ������ ������ ��
        if(currentCropField != null && currentCropField.GetComponent<CropField>().State == CropField.FieldState.Grown)
        {
            GameObject crop = currentCropField.GetComponent<CropField>().Crop;
            yield return new WaitForSeconds(1f);

            if (crop != null)
            {
                //���� �۹� ����
                GameObject.Destroy(crop);
                currentCropField.GetComponent<CropField>().State = CropField.FieldState.Empty;

                //���� �۹� null �ʱ�ȭ
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
            //�Է� ���� ������ ���
            yield return null;

            CropField.CropType type = CropField.CropType.None;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                type = CropField.CropType.Carrot;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                type = CropField.CropType.Corn;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                type = CropField.CropType.Cabbage;
            else if (Input.GetKeyDown(KeyCode.Escape)) //ESC������ ����
                isWaitingForSelecting = false;

            //�۹� ���õǾ�����
            if(type != CropField.CropType.None)
            {
                Animator anim = GetComponent<Animator>();
                anim.SetTrigger("pull_plant");
                StartCoroutine(PlantCrop(type));
                isWaitingForSelecting = false;
            }
        }
        farmingUI.SetActive(false);
    }
}
