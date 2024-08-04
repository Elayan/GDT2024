using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject Target = null;

    void Start()
    {
        if (Target == null)
        {
            Target = Camera.main.gameObject;
        }
    }
    void Update()
    {
        gameObject.transform.LookAt(Target.transform);
    }
}
