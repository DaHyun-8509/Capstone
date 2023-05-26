using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Harvester : MonoBehaviour
{
    private GameObject uiText; //��Ȯ�ϱ� �ؽ�Ʈ�� ������ ������Ʈ
    private GameObject currentCrop = null; //���� Ʈ���ſ� ���� �۹�

    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    void Update() //�� �����Ӹ���
    {
        //���� Ʈ���ſ� ���� �۹� �ִ� ���¿��� EŰ ���� ��
        if (currentCrop != null && Input.GetKeyDown(KeyCode.E))
        {
            //�ؽ�Ʈ ��Ȱ��ȭ
            uiText.gameObject.SetActive(false);

            //�۹� ��Ȯ
            HarvestCrop();
        }
    }

    private void OnTriggerEnter(Collider other) //Ʈ���� ���� ��
    {
        
        if (other.CompareTag("CropTrigger")&& currentCrop == null)
        {
            //���� Ʈ���ſ� ���� �۹��� ������ ���� ���� �۹��� ����
            currentCrop = other.gameObject.transform.parent.gameObject;

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
            currentCrop = null;
        }
    }

    private void HarvestCrop() //�۹� ��Ȯ
    {
        //�۹��� ��Ȯ ������ ������ ��
        if(currentCrop != null && currentCrop.GetComponent<Crop>().IsHarvestable)
        {
            //�۹� ��Ȱ��ȭ
            currentCrop.SetActive(false);

            //���� �۹� null �ʱ�ȭ
            currentCrop = null;
        }
    }
}
