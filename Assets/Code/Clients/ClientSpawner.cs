using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _clientPrefab;
    [SerializeField]
    private float _spawningTimer = 4f;
    [SerializeField]
    private Transform _spawnPos;


    private Randomizer _rand;
    private GameObject _activeClient;
    private float _timeSinceSpawn;

    // Start is called before the first frame update
    void Start()
    {
        _rand = Randomizer.Get();
        SpawnClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeSinceSpawn >= _spawningTimer)
        {
            SpawnClient();
        }
        else
        {
            _timeSinceSpawn += Time.deltaTime;
        }
    }

    private void SpawnClient()
    {
        GameObject seat = GetComponent<Counter>().TakeFreeSeat();
        if (seat == null)
        {
            Debug.Log("No more seat");
            return;
        }
        _timeSinceSpawn = 0f;
        if (_activeClient != null)
        {
            _activeClient.GetComponent<Client>().OnLeaving -= SpawnClient;
        }
        _activeClient = Instantiate(_clientPrefab, _spawnPos.position, _spawnPos.rotation);

        _activeClient.transform.SetParent(seat.transform);
        _activeClient.GetComponent<Client>().SetDestination(seat.transform.position);
        _activeClient.GetComponent<Client>().OnLeaving += SpawnClient;
    }
   
}
