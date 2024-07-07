using System.Linq;
using UnityEngine;

public abstract class EditorMachineBase : MachineBase
{
    private Transform _slot = null;
    private GameObject _itemOnSlot = null;

    private void Start()
    {
        _slot = Toolbox.FindInChildren(transform, "Slot");
        if (_slot == null)
            Debug.LogError("Missing child named 'Slot'!");
    }

    public override void Activate(Tray tray)
    {
        if (_itemOnSlot == null)
            TakeAndEdit(tray);
        else Dispense(tray);
    }

    private void TakeAndEdit(Tray tray)
    {
        if (tray.IsEmpty)
        {
            if (DebugLog)
                Debug.Log("Tray is empty, Editor Machine will do nothing.");
            return;
        }

        var editableItem = tray.Content.FirstOrDefault(c => IsItemEditable(c));
        if (editableItem == null)
        {
            if (DebugLog)
                Debug.Log("No editable item on Tray, Editor Machine will do nothing.");
            return;
        }

        var item = tray.Take(editableItem);
        item.transform.parent = _slot;
        item.transform.localPosition = Vector3.zero;

        EditItem(item);
    }

    private void Dispense(Tray tray)
    {
        if (tray.IsFull)
        {
            if (DebugLog)
                Debug.Log("Tray is full, Dispenser will not dispense.");
            return;
        }

        tray.Place(_itemOnSlot);
        _itemOnSlot = null;
    }

    /// <summary>
    /// Indicates if <paramref name="item"/> is valid to be edited by the Machine.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected virtual bool IsItemEditable(GameObject item)
    {
        return false;
    }

    /// <summary>
    /// Edit the item.
    /// </summary>
    /// <param name="item"></param>
    protected virtual void EditItem(GameObject item)
    {
    }
}
