using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _clientPrefab;
    [SerializeField]
    private List<Transform> _clientSpawnPosList;

    private Randomizer _rand;
    private GameObject _activeClient;

    // Start is called before the first frame update
    void Start()
    {
        _rand = Randomizer.Get();
        SpawnClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnClient()
    {
        int chosenPos = _rand.Next(_clientSpawnPosList.Count);
        if (_activeClient != null)
        {
            _activeClient.GetComponent<Client>().OnLeaving -= SpawnClient;
        }
        _activeClient = Instantiate(_clientPrefab, _clientSpawnPosList[chosenPos].position, _clientSpawnPosList[chosenPos].rotation);
        _activeClient.transform.SetParent(_clientSpawnPosList[chosenPos]);
        _activeClient.GetComponent<Client>().OnLeaving += SpawnClient;
    }
   
}
