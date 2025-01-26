using UnityEngine;

public class WaterTap : MonoBehaviour
{
    public Animator Tap;
    public ParticleSystem RunningWater;
    public AudioSource openSound;

    private bool _inUse = true;

    void Start()
    {
        RunningWater.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftHand") || other.gameObject.CompareTag("RightHand"))
        {
            if (_inUse)
            {
                Tap.SetBool("Open", true);
                Tap.SetBool("Closed", false);
                openSound.Play();
                RunningWater.Play();
                _inUse = false;
            }
            else
            {
                Tap.SetBool("Open", false);
                Tap.SetBool("Closed", true);
                openSound.Pause();
                RunningWater.Stop();
                _inUse = true;
            }
        }
    }
}
