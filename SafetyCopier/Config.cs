using Newtonsoft.Json;

namespace SafetyCopier;
public class Config
{
    public List<string> BackupLocations { get; set; }
    public static string Location { get; } = $"{GetAndCreateAppDataFolder()}config.json";
    public string UserName { get; set; }
    public string Url { get; set; }
    public string Password { get; set; }

    public void Save()
    {
        string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(Location, jsonString);
    }

    public void OverrideBackupLocations(List<string> newBackupLocations)
    {
        BackupLocations = newBackupLocations;
        Save();
    }

    public MyFTP GetFTP()
    {
        if (string.IsNullOrEmpty(Url))
        {
            Console.WriteLine("I can has ftp url?");
            Url = Console.ReadLine();
            Save();
        }
        if (string.IsNullOrEmpty(UserName))
        {
            Console.WriteLine("I can has username?");
            UserName = Console.ReadLine();
            Save();
        }
        if (string.IsNullOrEmpty(Password))
        {
            Console.WriteLine("I can has password?");
            Password = Console.ReadLine();
            Save();
        }
        return new MyFTP(Url, UserName, Password);
    }

    public static Config Load()
    {
        if (File.Exists(Location))
        {
            string jsonString = File.ReadAllText(Location);
            Config _config = JsonConvert.DeserializeObject<Config>(jsonString)!;
            return _config;
        }
        var _conf = new Config();
        _conf.Save();
        return _conf;
    }

    private static string GetAndCreateAppDataFolder(string folderName = "SafetyCopier")
    {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + folderName + Path.DirectorySeparatorChar;
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }
}
