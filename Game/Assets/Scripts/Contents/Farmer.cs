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
    private GameObject currentCropField = null; //���� Ʈ���ſ� ���� �۹� ����
    
    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    void Update() //�� �����Ӹ���
    {
        //���� Ʈ���ſ� ���� �۹� �ִ� ���¿��� EŰ ���� ��
        if (currentCropField != null && Input.GetKeyDown(KeyCode.E))
        {
            //�ؽ�Ʈ ��Ȱ��ȭ
            uiText.gameObject.SetActive(false);

            //�۹� ��Ȯ
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("pull_plant");


            if(currentCropField.GetComponent<CropField>().IsGrown == true)
            {
                StartCoroutine(HarvestCrop());
            }  
            else if(currentCropField.GetComponent<CropField>().Crop == null)
            {
                StartCoroutine(currentCropField.GetComponent<CropField>().PlantCrop("Prefabs/Farm/Cabbage"));
                StartCoroutine(currentCropField.GetComponent<CropField>().GrowToLv2AfterDelay());

            }
        }
    }

    private void OnTriggerEnter(Collider other) //Ʈ���� ���� ��
    {
      
        if (other.CompareTag("CropTrigger")&& currentCropField == null)
        {
            //���� Ʈ���ſ� ���� �ʵ� ����
            currentCropField = other.transform.parent.gameObject;

            //�ؽ�Ʈ Ȱ��ȭ

            //�۹��� �ڶ� ���¸�
            if(currentCropField.GetComponent<CropField>().IsGrown == true)
            {
                uiText.GetComponent<TextMeshProUGUI>().text = "��Ȯ�ϱ�[E]";
            }
            //�۹��� ������
            else if(currentCropField.GetComponent<CropField>().Crop == null)
            {
                uiText.GetComponent<TextMeshProUGUI>().text = "�ɱ�[E]";
            }

            uiText.gameObject.SetActive(true);
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

    private IEnumerator HarvestCrop() //�۹� ��Ȯ
    {
        //�۹��� ��Ȯ ������ ������ ��
        if(currentCropField != null && currentCropField.GetComponent<CropField>().IsGrown)
        {
            GameObject crop = currentCropField.GetComponent<CropField>().Crop;
            yield return new WaitForSeconds(1f);

            if (crop != null)
            {
                //���� �۹� ����
                GameObject.Destroy(crop);
                currentCropField.GetComponent<CropField>().IsGrown = false;


                //���� �۹� null �ʱ�ȭ
                currentCropField = null;
            }
        }
    }
}
