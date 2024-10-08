using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public bool DebugLog = false;
    [SerializeField]
    private Rigidbody _rb;
    public float Speed = 1f;
    [SerializeField]
    private float _rotationSpeed = 5f;

    private GameObject _camera = null;
    private Vector3 _movement = Vector3.zero;

    private List<MachineBase> _machinesAtReach = new List<MachineBase>();
    public List<Client> _clientsAtReach = new List<Client>();
    private Tray _tray = null;

    private void Start()
    {
        _camera = Camera.allCameras.FirstOrDefault(c => c.name == "Main Camera")?.gameObject;
        if (_camera == null)
            Debug.LogError("No camera called 'Main Camera' found!", gameObject);

        var collisionDetector = gameObject.GetComponentInChildren<CollisionDetector>();
        if (collisionDetector == null)
            Debug.LogError($"No CollisionDetector component found in {name}'s children!", gameObject);

        collisionDetector.OnTriggerEnterDelegate += OnTriggerEnter;
        collisionDetector.OnTriggerExitDelegate += OnTriggerExit;

        _tray = GetComponentInChildren<Tray>();
        if (_tray == null)
            Debug.LogError($"No Tray component found in {name}'s children!", gameObject);
        if (_rb == null)
            Debug.LogError($"Missing rigidbody on {name}", gameObject);
    }

    private void Update()
    {
     //   gameObject.transform.Translate(_movement * Speed * Time.deltaTime);
     if (_movement != Vector3.zero)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_movement, Vector3.up), Time.deltaTime * _rotationSpeed);
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(gameObject.transform.position + _movement * Speed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue value)
    {
        var vecValue = value.Get<Vector2>();

        // The simple way would be this
        //_movement.x = vecValue.x;
        //_movement.z = vecValue.y;

        // But it's comfier to move relatively to camera angle.
        // But since camera is angled down, I first have to project right/forward vector to a flat plan.
        // That being said, there are a few issues:
        //  - with keyboard, it's quickly annoying, since right/forward are never refresh while direction is maintained
        //  - with gamepad, there's a slight sliding at stick rebound
        _movement = Vector3.zero;
        _movement += _camera.transform.right * vecValue.x;
        _movement += _camera.transform.forward * vecValue.y;

        _movement.y = 0;

        if (DebugLog)
            Debug.Log($"movement X={_movement.x} Y={_movement.y} Z={_movement.z}", gameObject);
    }

    void OnAction()
    {
        // if decor is packed, we might need to activate the closer one :)
        if (_machinesAtReach.Count != 0)
        {
            _machinesAtReach[0].Activate(_tray);
        }
        else if (_clientsAtReach.Count != 0)
        {
            _clientsAtReach[0].Activate(_tray);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherMachine = other.gameObject.GetComponentInParent<MachineBase>();
        if (otherMachine != null)
        {
            if (!_machinesAtReach.Contains(otherMachine))
                _machinesAtReach.Add(otherMachine);
            return;
        }

        var otherClient = other.gameObject.GetComponentInParent<Client>();
        if (otherClient != null)
        {
            if (!_clientsAtReach.Contains(otherClient))
                _clientsAtReach.Add(otherClient);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var otherMachine = other.gameObject.GetComponentInParent<MachineBase>();
        if (otherMachine != null)
        {
            _machinesAtReach.Remove(otherMachine);
            return;
        }

        var otherClient = other.gameObject.GetComponentInParent<Client>();
        if (otherClient != null)
        {
            _clientsAtReach.Remove(otherClient);
            return;
        }
    }
}
