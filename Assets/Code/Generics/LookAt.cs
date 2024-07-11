using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject Target = null;

    void Update()
    {
        gameObject.transform.LookAt(Target.transform);
    }
}
