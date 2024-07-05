using UnityEngine;

public class MachineBase : MonoBehaviour
{
    public Color IndicatorColor;

    private GameObject _indicator;
    private Color _indicatorDefaultColor;

    void Start()
    {
        var collisionDetector = gameObject.GetComponentInChildren<CollisionDetector>();
        collisionDetector.OnTriggerEnterDelegate += OnTriggerEnter;
        collisionDetector.OnTriggerExitDelegate += OnTriggerExit;

        _indicator = Toolbox.FindInChildren(transform, "Indicator")?.gameObject;
        if (_indicator == null)
            Debug.LogError($"Machine {name} didn't find its Indicator!");

        _indicatorDefaultColor = _indicator.GetComponent<Renderer>().material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        _indicator.GetComponent<Renderer>().material.color = IndicatorColor;
    }

    private void OnTriggerExit(Collider other)
    {
        _indicator.GetComponent<Renderer>().material.color = _indicatorDefaultColor;
    }

    public virtual void Activate()
    {
        // base script do nothing, it has to be implemented!
    }
}
