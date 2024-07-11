using System.Linq;
using UnityEngine;

public abstract class EditorMachineBase : MachineBase
{
    private Transform _slot = null;
    private GameObject _itemOnSlot = null;

    protected override void Initialize()
    {
        base.Initialize();

        _slot = Toolbox.FindInChildren(transform, "Slot");
        if (_slot == null)
            Debug.LogError($"Missing 'Slot' in {name}'s children!", gameObject);
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
                Debug.Log("Tray is empty, Editor Machine will do nothing.", gameObject);
            return;
        }

        var editableItemsAndRecipes = tray.Content.ToDictionary(c => c, c => c.GetComponentInChildren<Recipe>());
        var editableItem = editableItemsAndRecipes.FirstOrDefault(c => IsItemEditable(c.Key, c.Value)).Key;
        if (editableItem == null)
        {
            if (DebugLog)
                Debug.Log("No editable item on Tray, Editor Machine will do nothing.", gameObject);
            return;
        }

        var item = tray.Take(editableItem);
        item.transform.parent = _slot;
        item.transform.localPosition = Vector3.zero;
        _itemOnSlot = item;

        EditItem(item, editableItemsAndRecipes[item]);
    }

    private void Dispense(Tray tray)
    {
        if (tray.IsFull)
        {
            if (DebugLog)
                Debug.Log("Tray is full, Dispenser will not dispense.", gameObject);
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
    protected virtual bool IsItemEditable(GameObject item, Recipe recipe)
    {
        return false;
    }

    /// <summary>
    /// Edit the item.
    /// </summary>
    /// <param name="item"></param>
    protected virtual void EditItem(GameObject item, Recipe recipe)
    {
    }
}
