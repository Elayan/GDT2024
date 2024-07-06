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
            Debug.LogError("No child 'Slots' found!");

        for (int i = 0; i < slotRoot.childCount; i++)
            _contentBySlots.Add(slotRoot.GetChild(i).gameObject, null);
    }

    public bool HasEmptySlot()
    {
        return _contentBySlots.Any(pair => pair.Value == null);
    }

    public void PutOnTray(GameObject prefab)
    {
        var firstEmptySlot = _contentBySlots.First(pair => pair.Value == null).Key;

        var item = Instantiate(prefab);
        item.transform.parent = firstEmptySlot.transform;
        item.transform.localPosition = Vector3.zero;
        _contentBySlots[firstEmptySlot] = item;
    }
}
