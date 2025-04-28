using UnityEngine;

public class LookAtAndFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Drag your camera here in the inspector
    public Vector3 offset; // Set the desired offset in the inspector

    void Update()
    {
        if (cameraTransform != null)
        {
            // Make the object look at the camera
            //transform.LookAt(cameraTransform);

            // Make the object follow the camera at a specific offset
            transform.position = cameraTransform.position + offset;
        }
    }
}
