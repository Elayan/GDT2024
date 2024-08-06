using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClientSpawner))]
public class Counter : MonoBehaviour
{
    private ClientSpawner _clientSpawner;

    [SerializeField]
    private List<Transform> _clientSpawnPosList;

    private List<bool> _usedSeat;
    private Randomizer _rand;

    // Start is called before the first frame update
    void Start()
    {
        _clientSpawner = GetComponent<ClientSpawner>();
        if (_clientSpawner == null)
            Debug.LogError("Error with ClientSpawner On");
        _rand = Randomizer.Get();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject TakeFreeSeat()
    {
        int nbTry = 0;
        int seat = _rand.Next(_clientSpawnPosList.Count);
        while (_clientSpawnPosList[seat].childCount != 0)
        {
            seat = (seat + 1) % _clientSpawnPosList.Count;
            nbTry++;
            if (nbTry >= _clientSpawnPosList.Count)
            {
                return null;
            }
        }
        return _clientSpawnPosList[seat].gameObject;
    }
}
