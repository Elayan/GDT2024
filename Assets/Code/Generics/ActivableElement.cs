using System;
using UnityEngine;

public abstract class ActivableElement : MonoBehaviour
{
    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        var collisionDetector = gameObject.GetComponentInChildren<CollisionDetector>();
        collisionDetector.OnTriggerEnterDelegate += OnTriggerEnter;
        collisionDetector.OnTriggerExitDelegate += OnTriggerExit;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }
}
