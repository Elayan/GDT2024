using UnityEngine;

public abstract class MachineBase : MonoBehaviour
{
    public bool DebugLog = false;

    public Color IndicatorColor;

    private GameObject _indicator;
    private Color _indicatorDefaultColor;

    void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        var collisionDetector = gameObject.GetComponentInChildren<CollisionDetector>();
        collisionDetector.OnTriggerEnterDelegate += OnTriggerEnter;
        collisionDetector.OnTriggerExitDelegate += OnTriggerExit;

        _indicator = Toolbox.FindInChildren(transform, "Indicator")?.gameObject;
        if (_indicator == null)
            Debug.LogError($"Missing 'Indicator' in {name}'s children!", gameObject);

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

    public virtual void Activate(Tray tray)
    {
        // base script do nothing, it has to be implemented!
    }
}
