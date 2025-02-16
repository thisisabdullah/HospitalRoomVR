using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public Transform ClipBoard;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        // Store the initial position and rotation of the clipboard
        if (ClipBoard != null)
        {
            initialPosition = ClipBoard.position;
            initialRotation = ClipBoard.rotation;
        }
        else
        {
            Debug.LogError("ClipBoard is not assigned in the Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClipBoard"))
        {
            // Reset the clipboard to its initial position and rotation
            if (ClipBoard != null)
            {
                ClipBoard.position = initialPosition;
                ClipBoard.rotation = initialRotation;
            }
        }
    }
}