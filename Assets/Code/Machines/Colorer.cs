using System.Linq;
using UnityEngine;

public class Colorer : EditorMachineBase
{
    public string ColorElementName = string.Empty;
    public Color Color = Color.white;

    public RecipeContent Content;

    protected override bool IsItemEditable(GameObject item, Drink recipe)
    {
        return recipe != null;
    }

    protected override void EditItem(GameObject item, Drink recipe)
    {
        recipe.MachinesWhoEdited.Add(this);

        var elementToColor = Toolbox.FindInChildren(item.transform, ColorElementName);
        if (elementToColor == null)
        {
            Debug.LogWarning($"No element named '{ColorElementName}' found in {item.name}'s children, Colorer will not color anything.", gameObject);
            return;
        }

        var renderer = elementToColor.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning($"Element '{ColorElementName}' in {item.name}'s children has no Renderer, Colorer will not color anything.", gameObject);
            return;
        }

        var mixedColor = ComputeMixedColor(recipe);
        renderer.material.color = mixedColor;
    }

    private Color ComputeMixedColor(Drink recipe)
    {
        var mixedColor = Color;
        var previousColorers = recipe.MachinesWhoEdited.OfType<Colorer>().Where(c => c.ColorElementName == ColorElementName).ToArray();
        foreach (var c in previousColorers)
        {
            mixedColor.r += c.Color.r;
            mixedColor.g += c.Color.g;
            mixedColor.b += c.Color.b;
        }

        mixedColor.r /= previousColorers.Length + 1;
        mixedColor.g /= previousColorers.Length + 1;
        mixedColor.b /= previousColorers.Length + 1;

        return mixedColor;
    }
}
