using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public bool DebugLog = false;
    public float Speed = 1f;

    private GameObject _camera = null;
    private Vector3 _movement = Vector3.zero;

    private void Start()
    {
        _camera = Camera.allCameras.FirstOrDefault(c => c.name == "Main Camera")?.gameObject;
        if (_camera == null)
            Debug.LogError("No camera called 'Main Camera' found!");
    }

    private void Update()
    {
        gameObject.transform.Translate(_movement * Speed * Time.deltaTime);
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
            Debug.Log($"movement X={_movement.x} Y={_movement.y} Z={_movement.z}");
    }
}
