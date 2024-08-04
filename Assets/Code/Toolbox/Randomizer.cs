

using System;

public class Randomizer
{
    // set this to any positive int and you'll have the same rolls each run!
    public const int DebugSeed = -1;

    static Randomizer()
    {
        _instance = new Randomizer();

        if (DebugSeed < 0)
            _instance._random = new Random();
        else _instance._random = new Random(DebugSeed);
    }

    private static Randomizer _instance = null;
    public static Randomizer Get() => _instance;

    private Random _random;

    public int Next() => _random.Next();
    public int Next(int maxExclusive) => _random.Next(maxExclusive);
    public int Next(int minInclusive, int maxExclusive) => _random.Next(minInclusive, maxExclusive);
}
