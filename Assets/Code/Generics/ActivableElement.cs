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

    /// <summary>
    /// What to do when something enters activable zone.
    /// </summary>
    /// <param name="other"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void OnTriggerEnter(Collider other)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// What to do when something leaves activable zone.
    /// </summary>
    /// <param name="other"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void OnTriggerExit(Collider other)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// What to do when activated.
    /// </summary>
    /// <param name="tray"></param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual void Activate(Tray tray)
    {
        throw new NotImplementedException();
    }
}
