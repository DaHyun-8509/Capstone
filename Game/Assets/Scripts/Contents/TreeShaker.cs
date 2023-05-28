using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeShaker : MonoBehaviour
{
    private GameObject uiText; //���� �ؽ�Ʈ�� ������ ������Ʈ
    private GameObject currentCrop = null; //���� Ʈ���ſ� ���� ����

    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    void Update() //�� �����Ӹ���
    {
        //���� Ʈ���ſ� ���� ���� �ִ� ���¿��� EŰ ���� ��
        if (currentCrop != null && Input.GetKeyDown(KeyCode.E))
        {
            //�ؽ�Ʈ ��Ȱ��ȭ
            uiText.gameObject.SetActive(false);

            //���� ����
            Rigidbody[] rigidbodys = currentCrop.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody r in rigidbodys)
            {
                r.useGravity = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other) //Ʈ���� ���� ��
    {

        if (other.CompareTag("TreeTrigger") && currentCrop == null)
        {
            //���� Ʈ���ſ� ���� �۹��� ������ ���� ���� �۹��� ����
            currentCrop = other.gameObject.transform.parent.gameObject;

            //�ؽ�Ʈ Ȱ��ȭ
            uiText.GetComponent<TextMeshProUGUI>().text = "����[E]";
            uiText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) //Ʈ���ſ��� �������� ��
    {
        if (other.CompareTag("TreeTrigger"))
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
        if (currentCrop != null && currentCrop.GetComponent<Crop>().IsHarvestable)
        {
            //�۹� ��Ȱ��ȭ
            currentCrop.SetActive(false);

            //���� �۹� null �ʱ�ȭ
            currentCrop = null;

            currentCrop.GetComponent<Crop>().IsHarvestable = false;
        }
    }
}
