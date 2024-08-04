using System;
using System.Collections;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Client : ActivableElement
{
    public bool DebugLog = false;

    [SerializeField]
    private ClientDialogueSO _clientDialogueSO;
    public Color ActivableBubbleColor = Color.white;
    public GameObject ActivableBubble = null;
    private Color _activableBubbleDefaultColor = Color.white;

    public GameObject BubbleText = null;
    private TextMeshPro _bubbleTextMesh = null;

    public float WaitTimeBeforeLeaving = 3.0f;
    [SerializeField]
    private float _speed = 5f;

    private Recipe _order = null;

    private Transform _exit = null;

    [Header("Timer")]
    [SerializeField]
    private float _waitingTime = 10f;
    [SerializeField]
    private GameObject _timerGO;

    private Image _imageTimer;
    private Randomizer _rand;
    private Coroutine _timerRoutine;

    public event Action OnLeaving;

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

        if (_exit == null)
        {
            FindExit();
        }

        if (_exit != null)
        {
            transform.Translate((_exit.position - transform.position).normalized * _speed * Time.deltaTime);
        }
        WaitTimeBeforeLeaving -= Time.deltaTime;
        if (WaitTimeBeforeLeaving < 0.0f)
        {
            OnLeaving?.Invoke();
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

        if (_timerGO == null)
        {
            Debug.LogError("No Timer found on Client", _timerGO);
        }
        else
        {
            _imageTimer = _timerGO.GetComponentInChildren<Image>();
            if (_imageTimer == null)
                Debug.LogError("Missig image time on TImerGameObject", _timerGO);
            _timerGO.SetActive(false);
        }

        _bubbleTextMesh.text = _clientDialogueSO.HelloPhrases[Randomizer.Get().Next(_clientDialogueSO.HelloPhrases.Count)];
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

        _timerRoutine = StartCoroutine(WaitingTimer());
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
        StopCoroutine(_timerRoutine);
        _timerGO.SetActive(false);
        tray.Remove(drinkMatchingOrder.gameObject);
        _bubbleTextMesh.text = _clientDialogueSO.WinPhrases[Randomizer.Get().Next(_clientDialogueSO.WinPhrases.Count)];

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
    private void FindExit()
    {
        _exit = GameObject.FindGameObjectWithTag("Exit").transform;
        if (_exit == null)
        {
            Debug.LogError("Missing exit position");
        }
    }
    public IEnumerator WaitingTimer()
    {
        _timerGO.SetActive(true);
        float time = 0f;
        while (time < _waitingTime)
        {
            _imageTimer.fillAmount = time / _waitingTime;
            if ((time / _waitingTime) > 0.5f)
            {
                _timerGO.gameObject.GetComponent<ShakeObject>().Shake();
            }

            if ((time / _waitingTime) > 0.75f)
            {
                _imageTimer.color = Color.red;
                _timerGO.gameObject.GetComponent<ShakeObject>().Shake();
            }
            time += Time.deltaTime;
            yield return null;
        }
        _bubbleTextMesh.text = _clientDialogueSO.LoosePhrases[Randomizer.Get().Next(_clientDialogueSO.LoosePhrases.Count)];

        _state = State.Leaving;
        _timerGO.SetActive(false);
    }
}
