using System;
using System.Linq;

namespace Sharpmine.Gen;

public static class GeneratorUtils
{

    public static string ToPascalCase(string input)
    {
        return string.Concat(
            input.Split(['/', '_', '.', ':'], StringSplitOptions.RemoveEmptyEntries)
                .Select(static word => char.ToUpper(word[0]) + word.Substring(1)));
    }

}
