using System;
using System.Web.Services;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;

[WebService(Namespace = "http://remwave.com/FileTransfer/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    public Service()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [XmlRoot("ShareInfo")]
    public class ShareInfo
    {
        [XmlElement("Size")]
        public long Size;
        [XmlElement("Username")]
        public String Username;
        public ShareInfo()
        {
        }

        public ShareInfo(String username, long size)
        {
            this.Size = size;
            this.Username = username;
        }
    }

    [WebMethod]
    public long GetFileSize(string username, string password, string id)
    {
        long size = 0;
        string FilePath = Server.MapPath(id);
        if (File.Exists(FilePath + ".xml"))
            using (FileStream fs = new FileStream(FilePath + ".xml", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    XmlSerializer des = new XmlSerializer(typeof(ShareInfo));
                    ShareInfo ret = (ShareInfo)des.Deserialize(fs);
                    fs.Close();
                    size = ret.Size;
                }
                catch
                {

                }
            }

        if (size > 0) return size;



        // check that requested file exists
        if (!File.Exists(FilePath))
            return 0;
        return new FileInfo(FilePath).Length;
    }

    [WebMethod]
    public byte[] GetFile(string username, string password, string id)
    {
        try
        {
            BinaryReader binReader = new
    BinaryReader(File.Open(Server.MapPath(id), FileMode.Open,
    FileAccess.Read));
            binReader.BaseStream.Position = 0;
            byte[] binFile =
    binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));
            binReader.Close();
            return binFile;
        }
        catch (Exception ex)
        {

            //TO DO Handle Error
        }
        return null;
    }

    [WebMethod]
    public byte[] GetFileChunk(string username, string password, string id, long offset, int size)
    {
        string FilePath = Server.MapPath(id);

        // check that requested file exists
        if (!File.Exists(FilePath))
            return null;

        long FileSize = new FileInfo(FilePath).Length;

        // if the requested Offset is larger than the file, quit.
        if (offset > FileSize)
            return null;

        // open the file to return the requested chunk as a byte[]
        byte[] TmpBuffer;
        int BytesRead;

        try
        {
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(offset, SeekOrigin.Begin);	// this is relevent during a retry. otherwise, it just seeks to the start
                TmpBuffer = new byte[size];
                BytesRead = fs.Read(TmpBuffer, 0, size);	// read the first chunk in the buffer (which is re-used for every chunk)
            }
            if (BytesRead != size)
            {
                // the last chunk will almost certainly not fill the buffer, so it must be trimmed before returning
                byte[] TrimmedBuffer = new byte[BytesRead];
                Array.Copy(TmpBuffer, TrimmedBuffer, BytesRead);
                return TrimmedBuffer;
            }
            else
                return TmpBuffer;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [WebMethod]
    public void PutFileSize(string username, string password, string id, long size)
    {
        string FilePath = Server.MapPath(id);
        using (FileStream fs = new FileStream(FilePath + ".xml", FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            try
            {
                XmlSerializer xser = new XmlSerializer(typeof(ShareInfo));
                StreamWriter sw = new StreamWriter(fs);
                xser.Serialize(sw, new ShareInfo(username,size));
                sw.Close();
            }
            catch
            {

            }
        }
    }

    [WebMethod]
    public void PutFile(string username, string password, string id, byte[] buffer)
    {
        try
        {
            BinaryWriter binWriter = new
     BinaryWriter(File.Open(Server.MapPath(id), FileMode.CreateNew,
     FileAccess.ReadWrite));
            binWriter.Write(buffer);
            binWriter.Close();
        }
        catch (Exception ex)
        {
            //TO DO Handle Error
        }
    }

    [WebMethod]
    public void PutFileChunk(string username, string password, string id, byte[] buffer, long offset)
    {
        String FilePath = Server.MapPath(id);
        if (offset == 0)	// new file, create an empty file
            File.Create(FilePath).Close();
        // open a file stream and write the buffer.  Don't open with FileMode.Append because the transfer may wish to start a different point
        using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
        {
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Write(buffer, 0, buffer.Length);
        }
    }

    [WebMethod]
    public string CheckFileHash(string username, string password, string id)
    {

        String FilePath = Server.MapPath(id);
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hash;
        using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
            hash = md5.ComputeHash(fs);
        return BitConverter.ToString(hash);
    }
}
