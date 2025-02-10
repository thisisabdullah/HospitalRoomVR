using UnityEngine;
using UnityEngine.Serialization;

public class HandSanitizer : MonoBehaviour
{
    public ParticleSystem SanitizerLiquid;
    public AudioSource SanitizerSound;

    void Start()
    {
        SanitizerLiquid.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftHand") || other.gameObject.CompareTag("RightHand"))
        {
            SanitizerLiquid.Play();
            SanitizerSound.Play();
            Invoke(nameof(DelayDisable), 0.7f);
        }
    }

    private void DelayDisable()
    {
        SanitizerLiquid.Stop();
        SanitizerSound.Pause();
    }
}
