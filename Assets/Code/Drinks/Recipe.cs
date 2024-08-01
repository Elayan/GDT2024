using System;

public enum RecipeContainer
{
    Glass,
}

public enum RecipeContent
{
    Juice,
    Vodka,
    Rhum
}

public enum RecipeExtra
{
    IceCubes,
}

public class Recipe
{
    private static RecipeContainer[] _allContainers = (RecipeContainer[])Enum.GetValues(typeof(RecipeContainer));
    private static RecipeContent[] _allContents = (RecipeContent[])Enum.GetValues(typeof(RecipeContent));
    private static RecipeExtra[] _allExtras = (RecipeExtra[])Enum.GetValues(typeof(RecipeExtra));

    // these should probably be different as we move further in levels
    public const int MinContent = 1;
    public const int MaxContent = 3;
    public const int MinExtra = 0;
    public const int MaxExtra = 1;

    public RecipeContainer Container { get; private set; }
    public RecipeContent[] Content { get; private set; }
    public RecipeExtra[] Extra { get; private set; }

    public static Recipe GenerateRecipe()
    {
        var recipe = new Recipe();
        var rand = Randomizer.Get();

        recipe.Container = _allContainers[rand.Next(_allContainers.Length)];

        var contentCount = rand.Next(MinContent, MaxContent);
        recipe.Content = new RecipeContent[contentCount];
        for (var i = 0; i < contentCount; i++)
            recipe.Content[i] = _allContents[rand.Next(_allContents.Length)];

        var extraCount = rand.Next(MinExtra, MaxExtra);
        recipe.Extra = new RecipeExtra[extraCount];
        for (var i = 0; i < extraCount; i++)
            recipe.Extra[i] = _allExtras[rand.Next(_allExtras.Length)];

        return recipe;
    }
}
