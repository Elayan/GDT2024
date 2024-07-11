using UnityEngine;

public class Client : ActivableElement
{
    public bool DebugLog = false;

    public Color ActivableBubbleColor = Color.white;
    public GameObject ActivableBubble = null;
    private Color _activableBubbleDefaultColor = Color.white;

    protected override void Initialize()
    {
        base.Initialize();

        var bubbleRenderer = ActivableBubble?.GetComponent<Renderer>();
        if (bubbleRenderer == null)
            Debug.LogError("No Activable Bubble with Renderer component!", gameObject);

        _activableBubbleDefaultColor = bubbleRenderer.material.color;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        ActivableBubble.GetComponent<Renderer>().material.color = ActivableBubbleColor;
    }

    protected override void OnTriggerExit(Collider other)
    {
        ActivableBubble.GetComponent<Renderer>().material.color = _activableBubbleDefaultColor;
    }
}
