namespace PureQL.CSharp.Model.Serialization.Tests;

/// <summary>
/// The fixtures of the PureQL specification, from the
/// <c>src/Tests/PureQL-Specification</c> submodule, which tracks its main branch.
/// </summary>
internal static class SpecificationFiles
{
    private static readonly string Root = Path.Combine(
        AppContext.BaseDirectory,
        "Specification"
    );

    public static IReadOnlyList<string> Valid()
    {
        return Names("samples/valid", "*.json", "samples/valid", "*.jsonc");
    }

    public static IReadOnlyList<string> Invalid()
    {
        return Names("samples/invalid", "*.jsonc");
    }

    public static TheoryData<string> Data(IEnumerable<string> names)
    {
        TheoryData<string> data = [];
        foreach (string name in names)
        {
            data.Add(name);
        }

        return data;
    }

    public static string Read(string name)
    {
        return File.ReadAllText(Path.Combine(Root, name));
    }

    private static List<string> Names(params string[] folderAndPattern)
    {
        List<string> names = [];
        for (int i = 0; i < folderAndPattern.Length; i += 2)
        {
            string folder = folderAndPattern[i];
            if (!Directory.Exists(Path.Combine(Root, folder)))
            {
                throw new DirectoryNotFoundException(
                    $"Specification fixtures not found in {Root}; "
                        + "run 'git submodule update --init'"
                );
            }

            IEnumerable<string> files = Directory
                .GetFiles(Path.Combine(Root, folder), folderAndPattern[i + 1])
                .Select(Path.GetFileName)
                .Order(StringComparer.Ordinal)!;
            foreach (string file in files)
            {
                names.Add($"{folder}/{file}");
            }
        }

        return names;
    }
}
