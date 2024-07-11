using System;
using UnityEngine;

public abstract class MachineBase : ActivableElement
{
    public bool DebugLog = false;

    public Color IndicatorColor;

    private GameObject _indicator;
    private Color _indicatorDefaultColor;

    protected override void Initialize()
    {
        _indicator = Toolbox.FindInChildren(transform, "Indicator")?.gameObject;
        if (_indicator == null)
            Debug.LogError($"Missing 'Indicator' in {name}'s children!", gameObject);

        _indicatorDefaultColor = _indicator.GetComponent<Renderer>().material.color;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        _indicator.GetComponent<Renderer>().material.color = IndicatorColor;
    }

    protected override void OnTriggerExit(Collider other)
    {
        _indicator.GetComponent<Renderer>().material.color = _indicatorDefaultColor;
    }

    public virtual void Activate(Tray tray)
    {
        throw new NotImplementedException();
    }
}
