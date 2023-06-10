using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject toActivate;

    [SerializeField] private Transform standingPoint;

    private Transform avatar;

    void Update()
    {
        // ESC Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Recover();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Managers.UI.SetInteractText("대화하기[E]");
        Managers.UI.EnableInteractText();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Managers.UI.DisableInteractText();

            avatar = other.transform;

            // disable player input
            avatar.GetComponent<PlayerController>().State = PlayerController.PlayerState.Interact;
            avatar.GetComponent<CharacterController>().enabled = false;

            //teleport the avartar to staning point
            avatar.position = standingPoint.position;
            avatar.rotation = standingPoint.rotation;

            // disable main cam, enable dialog cam
            mainCamera.SetActive(false);
            toActivate.SetActive(true);

            // gpt chat ui

            // display cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Managers.UI.DisableInteractText();
        }
    }


    public void Recover()
    {
        avatar.GetComponent<PlayerController>().State = PlayerController.PlayerState.Idle;
        avatar.GetComponent<CharacterController>().enabled = true;

        mainCamera.SetActive(true);
        toActivate.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // recover
}
