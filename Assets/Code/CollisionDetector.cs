using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public Action<Collider> OnTriggerEnterDelegate = null;
    public Action<Collider> OnTriggerExitDelegate = null;

    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterDelegate?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitDelegate?.Invoke(other);
    }
}
