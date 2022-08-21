namespace SafetyCopier;
public class Logger
{
    public string Location { get; } = $"{GetAndCreateAppDataFolder()}latest.txt";
    private List<ItemToLog> Items { get; set; } = new List<ItemToLog>();

    public Logger()
    {

    }

    public void Log(string message)
    {
        Items.Add(new ItemToLog(message));
    }

    public void Save()
    {
        string data = "";
        foreach (var item in Items)
        {
            data += $"{item}\n";
        }
        File.WriteAllText(Location, data);
    }

    private static string GetAndCreateAppDataFolder(string folderName = "SafetyCopier")
    {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + folderName + Path.DirectorySeparatorChar;
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }

    private class ItemToLog
    {
        private DateTime _created;
        private string _message;

        public ItemToLog(string message)
        {
            _created = DateTime.Now;
            _message = message;
        }

        public override string ToString()
        {
            return $"{_created.ToString("MM:dd:HH:mm:ss")}: {_message}";
        }
    }
}
