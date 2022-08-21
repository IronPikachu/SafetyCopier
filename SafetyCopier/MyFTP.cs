using System.Net;

namespace SafetyCopier;
public class MyFTP
{
    private string host;
    private string user;
    private string pass;

    public MyFTP(string url, string user, string password)
    {
        host = url;
        this.user = user;
        pass = password;
    }

    public async Task UploadFile(string fileLocation)
    {
        string fileName = Path.GetFileName(fileLocation);

        FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create($"{host}/{fileName}");

        ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
        ftpRequest.Credentials = new NetworkCredential(user, pass);

        using (FileStream fileStream = File.Open(fileLocation, FileMode.Open, FileAccess.Read))
        {
            using (Stream ftpStream = ftpRequest.GetRequestStream())
            {
                await fileStream.CopyToAsync(ftpStream);
            }
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            Console.WriteLine($"Upload File Complete, status {ftpResponse.StatusDescription}");
        }
    }
}
