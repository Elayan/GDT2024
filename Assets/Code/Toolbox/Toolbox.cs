using System;
using UnityEngine;

public static class Toolbox
{
    /// <summary>
    /// Crawl in children to find one with given name.
    /// This is ugly, please don't do like I do.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static Transform FindInChildren(Transform parent, string childName)
    {
        if (parent.name == childName)
            return parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            var correctChild = FindInChildren(parent.GetChild(i), childName);
            if (correctChild != null)
                return correctChild;
        }

        return null;
    }
}