using OpenAI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject toActivate;

    [SerializeField] private Transform standingPoint;
    [SerializeField] private CharacterType npcType;

    private Transform avatar;
    public Transform Avatar { get { return avatar; } }

    private bool isTalking = false;
    public bool Talking { get { return isTalking; } set { isTalking = value; } }

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
        if(other.CompareTag("Player"))
        {
            Managers.UI.SetInteractText("��ȭ�ϱ�[E]");
            Managers.UI.EnableInteractText();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Managers.UI.DisableInteractText();
            Talking = true;

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
            Managers.UI.DisableCanvas();

            // gpt chat ui

            // display cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            toActivate.GetComponentInChildren<ChatGPT>().NPCType = npcType;


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
        Managers.UI.EnableCanvas();

        Talking = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // recover
}
