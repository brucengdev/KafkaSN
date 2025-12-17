namespace Database;

public class Database
{
    private const string _dataFolder = "/mnt/data";
    public static void Append(string filename, string content)
    {
        File.AppendAllText(Path.Combine(_dataFolder, filename), content + Environment.NewLine);
    }
}