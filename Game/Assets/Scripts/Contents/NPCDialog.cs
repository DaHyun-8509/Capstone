using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject dialogCamera;

    [SerializeField] private Transform standingPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // disable main cam, enable dialog cam
            mainCamera.SetActive(false);
            dialogCamera.SetActive(true);

            //teleport the avartar to staning point
            Transform avatar = other.transform;
            avatar.position = standingPoint.position;
            avatar.rotation = standingPoint.rotation;

            // disable player input
            avatar.GetComponent<PlayerInput>().enabled = false;

            // gpt chat ui
        }
    }
    // recover
}
