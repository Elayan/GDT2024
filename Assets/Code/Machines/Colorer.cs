using System.Linq;
using UnityEngine;

public class Colorer : EditorMachineBase
{
    public string ColorElementName = string.Empty;
    public Color Color = Color.white;

    protected override bool IsItemEditable(GameObject item, Recipe recipe)
    {
        return recipe != null;
    }

    protected override void EditItem(GameObject item, Recipe recipe)
    {
        recipe.MachinesWhoEdited.Add(this);

        var elementToColor = Toolbox.FindInChildren(item.transform, ColorElementName);
        if (elementToColor == null)
        {
            Debug.LogWarning($"No element named '{ColorElementName}' found in {item.name}'s children, Colorer will not color anything.");
            return;
        }

        var renderer = elementToColor.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning($"Element '{ColorElementName}' in {item.name}'s children has no Renderer, Colorer will not color anything.");
            return;
        }

        var mixedColor = ComputeMixedColor(recipe);
        renderer.material.color = mixedColor;
    }

    private Color ComputeMixedColor(Recipe recipe)
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
