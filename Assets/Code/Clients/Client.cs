using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class Client : ActivableElement
{
    public bool DebugLog = false;

    public Color ActivableBubbleColor = Color.white;
    public GameObject ActivableBubble = null;
    private Color _activableBubbleDefaultColor = Color.white;

    public GameObject BubbleText = null;
    private TextMeshPro _bubbleTextMesh = null;

    public float WaitTimeBeforeLeaving = 3.0f;

    private Recipe _order = null;

    private enum State
    {
        WaitingForHello,
        Ordering,
        Leaving
    }
    private State _state = State.WaitingForHello;

    public void Update()
    {
        if (_state != State.Leaving)
            return;

        WaitTimeBeforeLeaving -= Time.deltaTime;
        if (WaitTimeBeforeLeaving < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        var bubbleRenderer = ActivableBubble?.GetComponent<Renderer>();
        if (bubbleRenderer == null)
            Debug.LogError("No Activable Bubble with Renderer component!", gameObject);

        _activableBubbleDefaultColor = bubbleRenderer.material.color;

        if (BubbleText == null)
            Debug.LogError("No Bubble Text provided!", gameObject);

        _bubbleTextMesh = BubbleText.GetComponent<TextMeshPro>();
        if (_bubbleTextMesh == null)
            Debug.LogError("No TextMeshPro component found on Bubble Text", BubbleText);

        _bubbleTextMesh.text = "?";
    }

    protected override void OnTriggerEnter(Collider other)
    {
        ActivableBubble.GetComponent<Renderer>().material.color = ActivableBubbleColor;
    }

    protected override void OnTriggerExit(Collider other)
    {
        ActivableBubble.GetComponent<Renderer>().material.color = _activableBubbleDefaultColor;
    }

    public override void Activate(Tray tray)
    {
        switch (_state)
        {
            case State.WaitingForHello:
                Activate_WaitingForHello();
                break;
            case State.Ordering:
                Activate_Ordering(tray);
                break;
        }
    }

    private void Activate_WaitingForHello()
    {
        _order = Recipe.GenerateRecipe();
        _bubbleTextMesh.text = RecipeToText();

        _state = State.Ordering;
    }

    private void Activate_Ordering(Tray tray)
    {
        var drinkMatchingOrder = tray.Drinks.FirstOrDefault(d => d.IsMatchingRecipe(_order, DebugLog));
        if (drinkMatchingOrder == null)
        {
            if (DebugLog)
                Debug.LogWarning("Client does not find their order on Tray!");
            return;
        }

        tray.Remove(drinkMatchingOrder.gameObject);
        _bubbleTextMesh.text = "Thx\nbye!";

        _state = State.Leaving;
    }

    private string RecipeToText()
    {
        var sb = new StringBuilder();
        sb.AppendLine(_order.Container.ToString());

        // read as contents[content] = how many of it
        var contents = _order.Content.GroupBy(c => c).ToDictionary(k => k.Key, v => v.Count());
        foreach (var content in contents)
            sb.AppendLine($"{content.Key.ToString()}{(content.Value <= 1 ? string.Empty : $" x{content.Value}")}");

        var extras = _order.Extra.GroupBy(c => c).ToDictionary(k => k.Key, v => v.Count());
        foreach (var extra in extras)
            sb.AppendLine($"{extra.Key.ToString()}{(extra.Value <= 1 ? string.Empty : $" x{extra.Value}")}");

        return sb.ToString();
    }
}
