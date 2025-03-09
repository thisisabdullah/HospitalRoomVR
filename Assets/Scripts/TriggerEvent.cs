using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent TriggerEnter;
    public UnityEvent DelayTriggerEnter;
    public string OtherGameobjectTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(OtherGameobjectTag))
        {
            TriggerEnter.Invoke();
            Invoke(nameof(DelayTrigger), 10f);
        }
    }

    private void DelayTrigger()
    {
        DelayTriggerEnter.Invoke();
    }
}
