using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tray : MonoBehaviour
{
    // Read as _contentBySlots[slot] = content
    private Dictionary<GameObject, GameObject> _contentBySlots = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        var slotRoot = Toolbox.FindInChildren(transform, "Slots");
        if (slotRoot == null)
            Debug.LogError("No child 'Slots' found!", gameObject);

        for (int i = 0; i < slotRoot.childCount; i++)
            _contentBySlots.Add(slotRoot.GetChild(i).gameObject, null);
    }

    public bool HasEmptySlot => _contentBySlots.Any(pair => pair.Value == null);
    public bool IsFull => !HasEmptySlot;
    public bool HasAnything => _contentBySlots.Any(pair => pair.Value != null);
    public bool IsEmpty => !HasAnything;

    /// <summary>
    /// Populated when called.
    /// </summary>
    public GameObject[] Content => _contentBySlots.Values.Where(v => v != null).ToArray();

    /// <summary>
    /// Populated from <see cref="Content"/> when called.
    /// </summary>
    public Drink[] Drinks => Content.Select(c => c.GetComponent<Drink>()).ToArray();

    /// <summary>
    /// Instantiates from prefab, then place it.
    /// </summary>
    /// <param name="prefab"></param>
    public void Put(GameObject prefab)
    {
        var item = Instantiate(prefab);
        Place(item);
    }

    /// <summary>
    /// Place <paramref name="item"/> on first empty slot found.
    /// </summary>
    /// <param name="item"></param>
    public void Place(GameObject item)
    {
        var firstEmptySlot = _contentBySlots.First(pair => pair.Value == null).Key;
        item.transform.parent = firstEmptySlot.transform;
        item.transform.localPosition = Vector3.zero;
        _contentBySlots[firstEmptySlot] = item;
    }

    /// <summary>
    /// Destroys <paramref name="item"/> from Tray.
    /// </summary>
    /// <param name="item"></param>
    public void Remove(GameObject item)
    {
        Destroy(item);
    }

    /// <summary>
    /// Destroys everything on every slot of the tray.
    /// </summary>
    public void ClearTray()
    {
        foreach (var pair in _contentBySlots)
        {
            if (pair.Value == null)
                continue;

            Destroy(pair.Value);
        }
    }

    /// <summary>
    /// Removes given item from its tray slot (doesn't destroy it!)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public GameObject Take(GameObject item)
    {
        var slot = _contentBySlots.First(pair => pair.Value == item);
        _contentBySlots[slot.Key] = null;
        return item;
    }
}
