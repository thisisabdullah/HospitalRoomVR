using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour
{
    public GameObject cube; // Assign your cube object in the Unity Editor
    public OVRHand leftHand;
    public OVRHand rightHand;
    private Vector3 lastScale;
    private bool isScaling = false;
    private Vector3 initialHandPosition;

    void Start()
    {
        lastScale = cube.transform.localScale;
    }

    void Update()
    {
        if (IsScalingGestureDetected())
        {
            if (!isScaling)
            {
                StartScaling();
            }
            ScaleCube();
        }
        else if (isScaling)
        {
            StopScaling();
        }
    }

    void StartScaling()
    {
        isScaling = true;
        initialHandPosition = GetHandPosition();
    }

    void ScaleCube()
    {
        Vector3 currentHandPosition = GetHandPosition();
        Vector3 scaleChange = currentHandPosition - initialHandPosition;

        // Determine which axis to scale
        Vector3 newScale = lastScale;
        if (Mathf.Abs(scaleChange.x) > Mathf.Abs(scaleChange.y) && Mathf.Abs(scaleChange.x) > Mathf.Abs(scaleChange.z))
        {
            newScale.x = lastScale.x * (1 + scaleChange.x * 5f);
        }
        else if (Mathf.Abs(scaleChange.y) > Mathf.Abs(scaleChange.x) && Mathf.Abs(scaleChange.y) > Mathf.Abs(scaleChange.z))
        {
            newScale.y = lastScale.y * (1 + scaleChange.y * 5f);
        }
        else
        {
            newScale.z = lastScale.z * (1 + scaleChange.z * 5f);
        }

        cube.transform.localScale = Vector3.Lerp(cube.transform.localScale, newScale, Time.deltaTime * 10f);
    }

    void StopScaling()
    {
        isScaling = false;
        lastScale = cube.transform.localScale;
        ResetCollider();
    }

    void ResetCollider()
    {
        BoxCollider cubeCollider = cube.GetComponent<BoxCollider>();
        if (cubeCollider != null)
        {
            cubeCollider.size = Vector3.one;
        }
    }

    Vector3 GetHandPosition()
    {
        if (rightHand.IsTracked)
        {
            return rightHand.transform.position;
        }
        else if (leftHand.IsTracked)
        {
            return leftHand.transform.position;
        }
        return Vector3.zero;
    }

    bool IsScalingGestureDetected()
    {
        return (rightHand.IsTracked && rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index)) ||
               (leftHand.IsTracked && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index));
    }
}
