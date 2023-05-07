using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float _speed = 10.0f;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyBoard; //중복 구독 방지
        Managers.Input.KeyAction += OnKeyBoard; //이벤트 구독
    }
    void Update()
    {
        
    }

    void OnKeyBoard()
    {
        //키보드 이동 처리 
    }
}
