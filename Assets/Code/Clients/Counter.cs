using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClientSpawner))]
public class Counter : MonoBehaviour
{


    private ClientSpawner _clientSpawner;

    // Start is called before the first frame update
    void Start()
    {
        _clientSpawner = GetComponent<ClientSpawner>();
        if (_clientSpawner == null)
            Debug.LogError("Error with ClientSpawner On");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
