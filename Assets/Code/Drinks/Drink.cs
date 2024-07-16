using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Drink : MonoBehaviour
{
    public List<MachineBase> MachinesWhoEdited = new List<MachineBase>();

    /// <summary>
    /// Please make this better, my enums are atrocious T-T
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
    public bool IsMatchingRecipe(Recipe recipe, bool debugLog = false)
    {
        var colorers = MachinesWhoEdited.OfType<Colorer>().ToArray();
        var adders = MachinesWhoEdited.OfType<Adder>().ToArray();
        if (colorers.Length != recipe.Content.Length)
        {
            if (debugLog)
                Debug.LogWarning($"Wrong order: found {colorers.Length} Colorers, expecting {recipe.Content.Length}.");
            return false;
        }
        if (adders.Length != recipe.Extra.Length)
        {
            if (debugLog)
                Debug.LogWarning($"Wrong order: found {adders.Length} Colorers, expecting {recipe.Extra.Length}.");
            return false;
        }

        return IsMatchingRecipeColorers(colorers, recipe, debugLog)
            && IsMatchingRecipeAdders(adders, recipe, debugLog);
    }

    private bool IsMatchingRecipeColorers(Colorer[] colorers, Recipe recipe, bool debugLog)
    {
        var colorersToVerify = colorers.ToList();
        foreach (var content in recipe.Content)
        {
            var match = colorersToVerify.FirstOrDefault(c => c.Content == content);
            if (match == null)
            {
                if (debugLog)
                    Debug.LogWarning($"Wrong order: didn't find a Colorer with tag {content}.");
                return false;
            }

            colorersToVerify.Remove(match);
        }

        return true;
    }

    private bool IsMatchingRecipeAdders(Adder[] adders, Recipe recipe, bool debugLog)
    {
        var addersToVerify = adders.ToList();
        foreach (var extra in recipe.Extra)
        {
            var match = addersToVerify.FirstOrDefault(c => c.Extra == extra);
            if (match == null)
            {
                if (debugLog)
                    Debug.LogWarning($"Wrong order: didn't find an Added with tag {extra}.");
                return false;
            }

            addersToVerify.Remove(match);
        }

        return true;
    }
}
