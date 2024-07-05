using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public bool DebugLog = false;
    public float Speed = 1f;

    private Vector3 _movement = Vector3.zero;

    private void Update()
    {
        gameObject.transform.Translate(_movement * Speed * Time.deltaTime);
    }

    void OnMove(InputValue value)
    {
        var vecValue = value.Get<Vector2>();
        _movement.x = vecValue.x;
        _movement.z = vecValue.y;

        if (DebugLog)
            Debug.Log($"movement X={_movement.x} Y={_movement.y} Z={_movement.z}");
    }
}
