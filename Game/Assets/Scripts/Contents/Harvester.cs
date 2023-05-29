using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Harvester : MonoBehaviour
{
    private GameObject uiText; //��Ȯ�ϱ� �ؽ�Ʈ�� ������ ������Ʈ
    private GameObject currentCropFiled = null; //���� Ʈ���ſ� ���� �۹� ����
    
    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    void Update() //�� �����Ӹ���
    {
        //���� Ʈ���ſ� ���� �۹� �ִ� ���¿��� EŰ ���� ��
        if (currentCropFiled != null && Input.GetKeyDown(KeyCode.E))
        {
            //�ؽ�Ʈ ��Ȱ��ȭ
            uiText.gameObject.SetActive(false);

            //�۹� ��Ȯ
            HarvestCrop();
        }
    }

    private void OnTriggerEnter(Collider other) //Ʈ���� ���� ��
    {
        
        if (other.CompareTag("CropTrigger")&& currentCropFiled == null)
        {
            //���� Ʈ���ſ� ���� �۹��� ������ ���� ���� �۹��� ����
            currentCropFiled = other.transform.parent.gameObject;

            //�ؽ�Ʈ Ȱ��ȭ
            uiText.GetComponent<TextMeshProUGUI>().text = "��Ȯ�ϱ�[E]";
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
            currentCropFiled = null;
        }
    }

    private void HarvestCrop() //�۹� ��Ȯ
    {
        //�۹��� ��Ȯ ������ ������ ��
        if(currentCropFiled != null && currentCropFiled.GetComponent<CropField>().IsGrown)
        {
            //���� �۹� �Ⱥ��̰�
            GameObject crop = currentCropFiled.transform.Find("Cabbage").gameObject;
            crop.SetActive(false);

            //���� �ð� �� �ڶ��
            StartCoroutine(currentCropFiled.GetComponent<CropField>().GrowTreeAfterDelay());

            //���� �۹� null �ʱ�ȭ
            currentCropFiled = null;
        }
    }
}
