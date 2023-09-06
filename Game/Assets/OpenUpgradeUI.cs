using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenUpgradeUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] canvases;
    bool playerIn = false;

    GameObject player;


    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            playerIn = true;
            player = collider.gameObject;
        }
    }

    private void Update()
    {
        if(playerIn && Input.GetKey(KeyCode.E))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            canvases[0].SetActive(true);
            Managers.Time.StopTime();
            player.GetComponent<PlayerController>().State = PlayerController.PlayerState.Interact;
            player.GetComponent<CharacterController>().enabled = false;
        }

        if(playerIn && Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            foreach (GameObject go in canvases)
            {
                go.SetActive(false);
            }
            Managers.Time.RunTime();
            player.GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(playerIn && collider.CompareTag("Player"))
        {
            playerIn = false;
        }
    }

}
