using System;
using UnityEngine;

public class PinchDrag : MonoBehaviour
{
    public OVRHand RightHand;
    public OVRHand LeftHand;
    public Vector3 Direction;
    private CubeFaceExtender _cubeFaceExtender;

    private void Start()
    {
        _cubeFaceExtender = GetComponent<CubeFaceExtender>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("OVRHand_R"))
        {
            print("Collided");
            if (RightHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                _cubeFaceExtender.meshRenderer.enabled = true;
                _cubeFaceExtender.ExtendFace(Direction, _cubeFaceExtender.ExtendAmount); 
            }
        }

        if (other.gameObject.CompareTag("OVRHand_L"))
        {
            if (LeftHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                _cubeFaceExtender.DeExtendFace(Direction, _cubeFaceExtender.ExtendAmount); 
            }
            
        }
    }
}
