using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCameraController : MonoBehaviour
{
    Transform _player;

    

    private float _lookAtHeight;

    void Start()
    {
        _player = transform.parent;
        _lookAtHeight = GetComponentInParent<CharacterController>().height / 2;
    }


    public void Rotate()
    {
       


        
    }
   
}
