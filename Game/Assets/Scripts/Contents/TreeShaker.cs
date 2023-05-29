using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeShaker : MonoBehaviour
{
    private GameObject uiText; //흔들기 텍스트를 보여줄 오브젝트
    private GameObject currentCrop = null; //현재 트리거에 잡힌 나무

    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    void Update() //매 프레임마다
    {
        //현재 트리거에 잡힌 나무 자란 상태에서 E키 누를 때
        if (currentCrop != null && currentCrop.GetComponent<TreeField>().IsGrown)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                //텍스트 비활성화
                uiText.gameObject.SetActive(false);
                ShakeTree();
            }
        }
    }

    private void OnTriggerEnter(Collider other) //트리거 잡힐 때
    {

        if (other.CompareTag("TreeTrigger") && currentCrop == null)
        {
            //현재 트리거에 잡힌 나무가 없으면 지금 잡힌 나무를 저장
            currentCrop = other.gameObject.transform.parent.gameObject;

            if (currentCrop.GetComponent<TreeField>().IsGrown)
            {
                //텍스트 활성화
                uiText.GetComponent<TextMeshProUGUI>().text = "흔들기[E]";
                uiText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) //트리거에서 빠져나올 때
    {
        if (other.CompareTag("TreeTrigger"))
        {
            //텍스트 비활성화
            uiText.gameObject.SetActive(false);

            //현재 잡힌 나무 null로 초기화
            currentCrop = null;
        }
    }

    private void ShakeTree()
    {
        TreeField treefield = currentCrop.GetComponentInChildren<TreeField>();

        //나무 흔들기
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
