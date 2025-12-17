namespace Database;

public class Database
{
    private const string _dataFolder = "/mnt/data";
    public static void Append(string filename, string content)
    {
        File.AppendAllText(Path.Combine(_dataFolder, filename), content + Environment.NewLine);
    }

    public static void Replace(string filename, string content)
    {
        File.WriteAllText(Path.Combine(_dataFolder, filename), content);
    }

    public static string ReadFile(string filename)
    {
        var fullPath = Path.Combine(_dataFolder, filename);
        if(!File.Exists(fullPath)) { return string.Empty; }
        return File.ReadAllText(fullPath);
    }
}