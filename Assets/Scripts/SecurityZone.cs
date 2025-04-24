using System;
using UnityEngine;

public class SecurityZone : MonoBehaviour 
{
    public event Action EnteredThief;
    public event Action ExitedThief;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Thief>(out _))
        {
            EnteredThief?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Thief>(out _))
        {
            ExitedThief?.Invoke();
        }
    }
}