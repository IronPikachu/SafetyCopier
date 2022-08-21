using SafetyCopier;

if (args.Length == 0)
{
    // idk
}
else if (args.Length == 1)
{
    Backuper backuper = new Backuper(args[0]);

    await backuper.Start();
}
else
{
    string testFile = args[0];
    List<string> backupFolders = new List<string>();

    for (int i = 1; i < args.Length; i++)
    {
        backupFolders.Add(args[i]);
    }

    Backuper backuper = new Backuper(testFile, backupFolders);

    await backuper.Start();
}
