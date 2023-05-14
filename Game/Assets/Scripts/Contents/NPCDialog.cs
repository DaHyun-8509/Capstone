using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject toActivate;

    [SerializeField] private Transform standingPoint;

    private Transform avatar;
    
    private async void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            avatar = other.transform;

            // disable player input
            avatar.GetComponent<PlayerController>().enabled = false;
            avatar.GetComponent<CharacterController>().enabled = false;

            await Task.Delay(50);

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

    public void Recover()
    {
        avatar.GetComponent<PlayerController>().enabled = true;
        avatar.GetComponent<CharacterController>().enabled = true;

        mainCamera.SetActive(true);
        toActivate.SetActive(false);


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // recover
}
