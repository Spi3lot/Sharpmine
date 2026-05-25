using System;
using System.Linq;

namespace Sharpmine.Gen;

public static class GeneratorUtils
{

    public static string ToPascalCase(string input)
    {
        return string.Join(
            string.Empty,
            input.Split(['/', '_', '.', ':'], StringSplitOptions.RemoveEmptyEntries)
                .Select(static word => char.ToUpper(word[0]) + word.Substring(1)));
    }

}
