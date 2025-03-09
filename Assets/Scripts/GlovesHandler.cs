using System;
using UnityEngine;

public class GlovesHandler : MonoBehaviour
{
    public SkinnedMeshRenderer RightHand;
    public SkinnedMeshRenderer LeftHand;
    public GameObject TextNotifier;
    public AudioClip SE;
    public AudioSource Sound;
    public Material[] NewMaterials;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftHand") || other.gameObject.CompareTag("RightHand"))
        {
            TextNotifier.SetActive(true);
            Invoke(nameof(DelayChange), 1f);
        }
    }

    private void DelayChange()
    { 
        Sound.PlayOneShot(SE);
        RightHand.materials = NewMaterials;
        LeftHand.materials = NewMaterials;
        TextNotifier.SetActive(false);
    }
}
