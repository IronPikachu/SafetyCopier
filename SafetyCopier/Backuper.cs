namespace SafetyCopier;
public class Backuper
{
    public string OriginFile { get; private set; }
    public List<string> BackupLocations { get; private set; }

    private MyFTP ftp;
    private Logger? logger;

    public Backuper(string originFile)
    {
        OriginFile = originFile;
        var config = Config.Load();
        BackupLocations = config.BackupLocations;
        ftp = config.GetFTP();
    }

    public Backuper(string originFile, List<string> backups)
    {
        OriginFile = originFile;
        var config = Config.Load();
        config.OverrideBackupLocations(backups);
        BackupLocations = config.BackupLocations;
        config.Save();
        ftp = config.GetFTP();
    }

    public async Task Start()
    {
        logger = new Logger();
        string fileName = Path.GetFileName(OriginFile);
        try
        {
            foreach (var backup in BackupLocations)
            {

                File.Copy(OriginFile, $"{backup}{Path.DirectorySeparatorChar}{fileName}", true);
                logger.Log($"Successfully copied {OriginFile} to {backup}!");
            }
            logger.Log("DONE COPYING LOCALLY!");
        }
        catch (Exception e)
        {
            logger.Log($"Exited with error message: {e.Message}");
        }
        await Upload();
        logger.Save();
    }

    private async Task Upload()
    {
        try
        {
            await ftp.UploadFile(OriginFile);
            logger!.Log($"DONE UPLOADING TO HOST!");
        }
        catch(Exception e)
        {
            logger!.Log($"FTP Client encountered an error: {e.Message}");
        }
    }
}
