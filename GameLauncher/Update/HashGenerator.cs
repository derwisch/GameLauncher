#if !HashGenerator
using GameLauncher.UI;
using System.Windows.Forms;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace GameLauncher.Update
{
    public class HashGenerator
    {
#if !HashGenerator
        public static string RootDirectory = new FileInfo(Application.ExecutablePath).DirectoryName;
#else
        public static string RootDirectory = "";
#endif
        public static ContentFile CurrentContentFile;

        public static bool CheckFile(String path)
        {
            if (CurrentContentFile == null)
                return true;
            string relPath = path.Replace(RootDirectory, String.Empty);
            FileStateInfo[] array = ((IEnumerable<FileStateInfo>)CurrentContentFile.Files).Select(f => f).Where(f => f.FileName == relPath).ToArray();
            if (array.Length == 0)
                return true;
            FileStateInfo fileStateInfo = array[0];
            FileInfo fileInfo = new FileInfo(path);
            long length = fileInfo.Length;
            if (fileStateInfo.FileSize != length)
                return false;
            DateTime lastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            if (fileStateInfo.LastWrite != lastWriteTimeUtc)
                return false;
            using (FileStream fileStream = fileInfo.OpenRead())
            {
                byte[] hash = new MD5CryptoServiceProvider().ComputeHash(fileStream);
                fileStream.Close();
                return fileStateInfo.MD5Hash == hash;
            }
        }

        public class FileStateInfo
        {
            public string FileName;
            public long FileSize;
            public byte[] MD5Hash;
            public DateTime LastWrite;

            public bool CheckLocal()
            {
                string filePath = RootDirectory + FileName;
                if (!File.Exists(filePath))
                    return false;
                FileInfo fileInfo = new FileInfo(filePath);
                if (FileSize != fileInfo.Length)
                    return false;
                using (FileStream fileStream = fileInfo.OpenRead())
                {
                    byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream)fileStream);
                    fileStream.Close();
                    return ((IEnumerable<byte>)MD5Hash).SequenceEqual(hash);
                }
            }

#if HashGenerator
            public static FileStateInfo FromFile(FileInfo info)
            {
                var result = new FileStateInfo();

                result.FileName = info.FullName;
                result.FileSize = info.Length;
                result.LastWrite = info.LastWriteTimeUtc;
                using (FileStream fileStream = info.OpenRead())
                {
                    byte[] hash = new MD5CryptoServiceProvider().ComputeHash(fileStream);
                    fileStream.Close();
                    result.MD5Hash = hash;
                }

                return result;
            }
#endif
        }

        public class ContentFile
        {
            public string GameVersion = string.Empty;
            public FileStateInfo[] Files = new FileStateInfo[0];
            [NonSerialized]
            public List<FileStateInfo> FileList = new List<FileStateInfo>();

            public static ContentFile FromFile(string path)
            {
                try
                {
                    ContentFile contentFile = new ContentFile();
                    using (FileStream fileStream = new FileStream(path, FileMode.Open))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(FileStateInfo[]));
                        contentFile.Files = (FileStateInfo[])xmlSerializer.Deserialize((Stream)fileStream);
                        fileStream.Close();
                    }
                    using (FileStream fileStream = new FileStream(path.Replace(".xml", ".version"), FileMode.Open))
                    {
                        StreamReader streamReader = new StreamReader(fileStream);
                        while (!streamReader.EndOfStream)
                        {
                            string line = streamReader.ReadLine();
                            if (line.StartsWith("GameVersion="))
                                contentFile.GameVersion = line.Replace("GameVersion=", String.Empty);
                        }
                        fileStream.Close();
                    }
                    return contentFile;
                }
                catch (Exception ex)
                {
#if !HashGenerator
                    MessageBox.Show($"{Localization.GetText("HashGenerator.Error.CannotReadContentFile")}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
#else
                    Console.WriteLine("Can't read content file");
#endif
        }
                return null;
            }

            public void GenerateXml(string path)
            {
                Files = FileList.ToArray();
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    new XmlSerializer(typeof(FileStateInfo[])).Serialize(fileStream, Files);
                    fileStream.Close();
                }
                string[] contents = new string[1]
                {
                    String.Format("GameVersion={0}", GameVersion)
                };
                File.WriteAllLines(path.Replace(".xml", ".version"), contents);
            }
        }
    }
}
