using UnityEngine;
using System.Collections;

public class DoorHandler : MonoBehaviour
{
    public Transform door; // Assign the door GameObject
    public float openAngle = 70f;
    public float speed = 0.65f;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = door.rotation;
        openRotation = door.rotation * Quaternion.Euler(0, 0, openAngle);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftHand") || other.gameObject.CompareTag("RightHand"))
        {
            ToggleDoor();
        }
    }

    [ContextMenu("Open")]
    public void ToggleDoor()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(isOpen ? closedRotation : openRotation));
        isOpen = !isOpen;
    }

    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(door.rotation, targetRotation) > 0.1f)
        {
            door.rotation = Quaternion.Slerp(door.rotation, targetRotation, Time.deltaTime * speed);
            yield return null;
        }
        door.rotation = targetRotation;
    }
}