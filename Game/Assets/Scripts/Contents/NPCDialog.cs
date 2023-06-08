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
    private GameObject uiText;

    void Update()
    {
        // ESC Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Recover();
        }
    }

    void Awake()
    {
        uiText = GameObject.Find("UI_InteractionText");
    }

    private void OnTriggerEnter(Collider other)
    {
        uiText.GetComponent<TextMeshProUGUI>().text = "대화하기[E]";
        uiText.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            uiText.gameObject.SetActive(false);

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
            uiText.gameObject.SetActive(false);
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
