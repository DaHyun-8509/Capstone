using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour
{
    CharacterController _controller;

    //�̵�
    public float _walkSpeed = 3.0f;
    public float _runSpeed = 6.0f;

    //ȸ��
    public float _turnSpeed = 10.0f;
    public float _maxUpAngle = 80f;
    public float _maxDownAngle = 30f;
    private float _xRotation = 0f;
    private float _yRotation = 0f;
    private float _cameraDistance = 6f;

    public enum PlayerState
    {
        Idle, 
        Walk,
        Run,
        Interact
    }

    PlayerState _state = PlayerState.Idle;
    public PlayerState State { get { return _state; } set { _state = value; }}

    void UpdateIdle()
    {
        Move(_walkSpeed);
        Rotate();
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("move_speed", 0);
    }

    void UpdateWalk()
    {
        Move(_walkSpeed);
        Rotate();

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("move_speed", _walkSpeed);
    }

    void UpdateRun()
    {
        Move(_runSpeed);
        Rotate();

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("move_speed", _runSpeed);
    }
    
    void UpdateInteract()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("move_speed", 0);
    }


    void Move(float speed) //Ű���� �Է¿� ���� �÷��̾� �̵�
    {
        if (_controller.enabled == false)
            return;
        //Ű �Է� �̺�Ʈ -> Ű���� �̵� ó��
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        _controller.Move(transform.TransformDirection(moveDirection * speed * Time.deltaTime));

        //�������� ������ Walk ������ Idle
        if (moveDirection.magnitude > 0.0001f)
            _state = PlayerState.Walk;
        else
            _state = PlayerState.Idle;

        if(Input.GetKey(KeyCode.LeftShift))
            _state = PlayerState.Run;
    }

    void Rotate()//���콺 �̵��� ���� �þ� ȸ��
    {
        float mouseX = Input.GetAxis("Mouse X") * _turnSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * _turnSpeed;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _maxDownAngle, _maxUpAngle);
        _yRotation += mouseX;

        Quaternion yRotation = Quaternion.Euler(0f, _yRotation, 0f);
        Quaternion xRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.localRotation = yRotation;
        Camera.main.transform.localRotation = xRotation * yRotation;

        float y = Mathf.Sin(_xRotation * Mathf.Deg2Rad) * _cameraDistance;
        float z = Mathf.Cos(_xRotation * Mathf.Deg2Rad) * _cameraDistance;

        Camera.main.transform.position = transform.position + transform.TransformVector(0f, y, -z);
        Camera.main.transform.LookAt(transform.position + new Vector3(0f, _controller.height / 2, 0f));

    }


    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }
    void Update()
    {

        //�÷��̾� ���� ��ȭ
        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Walk:
                UpdateWalk();
                break;
            case PlayerState.Run:
                UpdateRun();
                break;
            case PlayerState.Interact:
                UpdateInteract();
                break;
        }
    }

}
