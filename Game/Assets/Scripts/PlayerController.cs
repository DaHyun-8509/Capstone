using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float _speed = 10.0f;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyBoard; //�ߺ� ���� ����
        Managers.Input.KeyAction += OnKeyBoard; //�̺�Ʈ ����
    }
    void Update()
    {
        
    }

    void OnKeyBoard()
    {
        //Ű���� �̵� ó�� 
    }
}
