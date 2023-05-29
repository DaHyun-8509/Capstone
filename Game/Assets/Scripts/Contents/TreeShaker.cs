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
        //���� Ʈ���ſ� ���� ���� �ڶ� ���¿��� EŰ ���� ��
        if (currentCrop != null && currentCrop.GetComponent<TreeField>().IsGrown)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                //�ؽ�Ʈ ��Ȱ��ȭ
                uiText.gameObject.SetActive(false);
                ShakeTree();
            }
        }
    }

    private void OnTriggerEnter(Collider other) //Ʈ���� ���� ��
    {

        if (other.CompareTag("TreeTrigger") && currentCrop == null)
        {
            //���� Ʈ���ſ� ���� ������ ������ ���� ���� ������ ����
            currentCrop = other.gameObject.transform.parent.gameObject;

            if (currentCrop.GetComponent<TreeField>().IsGrown)
            {
                //�ؽ�Ʈ Ȱ��ȭ
                uiText.GetComponent<TextMeshProUGUI>().text = "����[E]";
                uiText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) //Ʈ���ſ��� �������� ��
    {
        if (other.CompareTag("TreeTrigger"))
        {
            //�ؽ�Ʈ ��Ȱ��ȭ
            uiText.gameObject.SetActive(false);

            //���� ���� ���� null�� �ʱ�ȭ
            currentCrop = null;
        }
    }

    private void ShakeTree()
    {
        TreeField treefield = currentCrop.GetComponentInChildren<TreeField>();

        //���� ����
        if(treefield.GetComponent<TreeField>().IsGrown)
        {
            Rigidbody[] rigidbodys = treefield.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody r in rigidbodys)
            {
                r.useGravity = true;
            }

            BoxCollider[] childrenBox = treefield.GetComponentsInChildren<BoxCollider>();
         
            foreach (BoxCollider box in childrenBox)
            {
                box.enabled = true;
            }

            StartCoroutine(treefield.GrowTreeAfterDelay());
            treefield.IsGrown = false;
        }
    }
}
