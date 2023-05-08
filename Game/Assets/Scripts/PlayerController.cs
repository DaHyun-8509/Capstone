using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{

    public float _moveSpeed = 10.0f;
    public float turnSpeed = 10.0f;
    public float maxLookUpAngle = 80f;
    public float maxLookDownAngle = -80f;

    private float _xRotation = 0f;
    private float _yRotation = 0f;

    CharacterController _controller;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyBoard; //중복 구독 방지
        Managers.Input.KeyAction += OnKeyBoard; //이벤트 구독

        _controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        //마우스 이동에 따른 시야 회전
        float mouseX = Input.GetAxis("Mouse X") * turnSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * turnSpeed;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, maxLookDownAngle, maxLookUpAngle);
        _yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    void OnKeyBoard()
    {
        //키 입력 이벤트 -> 키보드 이동 처리
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        _controller.Move(transform.TransformDirection(moveDirection * _moveSpeed * Time.deltaTime));

        transform.position = new Vector3(transform.position.x, 3.647f, transform.position.z);
    }

}
