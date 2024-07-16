using UnityEngine;

public class Adder : EditorMachineBase
{
    public GameObject AddPrefab = null;
    public string AddOnElementName = string.Empty;

    public RecipeExtra Extra;

    protected override bool IsItemEditable(GameObject item, Drink recipe)
    {
        return recipe != null && !recipe.MachinesWhoEdited.Contains(this);
    }

    protected override void EditItem(GameObject item, Drink recipe)
    {
        recipe.MachinesWhoEdited.Add(this);

        var addSlot = Toolbox.FindInChildren(item.transform, AddOnElementName);
        if (addSlot == null)
        {
            Debug.LogWarning($"No '{AddOnElementName}' found in {item.name}'s children, Adder will not add anything!", gameObject);
            return;
        }

        var addItem = Instantiate(AddPrefab, addSlot);
    }
}
