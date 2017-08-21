namespace Les.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class FileUtil
    {
        private const int BUFFER_SIZE = 0x200000;
        public const string DEFAULT_ENCODE = "GBK";
        private const int FO_DELETE = 3;

        public static string AbsolutePathOf(string pathName)
        {
            return AbsolutePathOf(pathName, true);
        }

        public static string AbsolutePathOf(string pathName, bool bUseExeDir)
        {
            if (!IsAbsolutePath(pathName))
            {
                if (bUseExeDir)
                {
                    pathName = GetExeDirFileName(pathName);
                    return pathName;
                }
                pathName = GetCurrentDirFileName(pathName);
            }
            return pathName;
        }

        public static void AppendToTxtFile(string fileName, string s)
        {
            AppendToTxtFile(fileName, s, "GBK");
        }

        public static void AppendToTxtFile(string fileName, string s, string codeName)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                File.AppendAllText(fileName, s);
            }
            else
            {
                File.AppendAllText(fileName, s, Encoding.GetEncoding(codeName));
            }
        }

        public static bool CanOpen(string fileName)
        {
            bool flag;
            try
            {
                using (File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    flag = true;
                }
            }
            catch (IOException)
            {
                flag = false;
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public static string ChangeDir(string fileName, string destDir)
        {
            return GetFileName(destDir, GetFileName(fileName));
        }

        public static string ChangeExt(string pathName, string ext)
        {
            int num = pathName.LastIndexOf('/');
            int num2 = pathName.LastIndexOf('\\');
            int length = pathName.LastIndexOf('.');
            if (((length > 0) && (length > num)) && (length > num2))
            {
                return (pathName.Substring(0, length) + "." + ext);
            }
            return (pathName + "." + ext);
        }

        public static void CopyData(Stream input, Stream output)
        {
            int count = -1;
            byte[] buffer = new byte[0x200000];
            while ((count = input.Read(buffer, 0, 0x200000)) > 0)
            {
                output.Write(buffer, 0, count);
            }
        }

        public static bool CopyFile(string srcFileName, string destFileName)
        {
            CreateDir2(DirectoryOf(destFileName), true);
            return CopyFile(srcFileName, destFileName, true);
        }

        public static bool CopyFile(string srcFileName, string destFileName, bool overwrite)
        {
            if (File.Exists(destFileName))
            {
                if (!overwrite)
                {
                    return false;
                }
                FileAttributes fileAttributes = File.GetAttributes(destFileName) & ~(FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly);
                File.SetAttributes(destFileName, fileAttributes);
            }
            try
            {
                File.Copy(srcFileName, destFileName, true);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (ArgumentNullException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (FileNotFoundException)
            {
            }
            catch (IOException)
            {
            }
            catch (NotSupportedException)
            {
            }
            return false;
        }

        public static void CopyFileTo(string fileName, string destDir)
        {
            CopyFileTo(fileName, destDir, true);
        }

        public static void CopyFileTo(string fileName, string destDir, bool overwrite)
        {
            string destFileName = GetFileName(destDir, FileNameOf(fileName));
            CopyFile(fileName, destFileName, overwrite);
        }

        public static bool CreateDir(string szDir)
        {
            if (szDir == null)
            {
                throw new ArgumentNullException("szDir");
            }
            szDir = szDir.Trim();
            if (szDir.Length > 0)
            {
                try
                {
                    Directory.CreateDirectory(szDir);
                    return true;
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (ArgumentNullException)
                {
                }
                catch (ArgumentException)
                {
                }
                catch (PathTooLongException)
                {
                }
                catch (DirectoryNotFoundException)
                {
                }
                catch (NotSupportedException)
                {
                }
                catch (IOException)
                {
                }
            }
            return false;
        }

        public static bool CreateDir2(string szDir, bool bRecurse)
        {
            if (szDir == null)
            {
                throw new ArgumentNullException("szDir");
            }
            if (bRecurse)
            {
                szDir = szDir.Trim();
                if (szDir.Length <= 0)
                {
                    return false;
                }
                if (szDir[szDir.Length - 1] == '\\')
                {
                    szDir = szDir.Substring(0, szDir.Length - 1);
                }
                if (szDir[szDir.Length - 1] == ':')
                {
                    return true;
                }
                if (IsDirectory(szDir))
                {
                    return true;
                }
                int length = szDir.LastIndexOf('\\');
                if (!CreateDir2(szDir.Substring(0, length), true))
                {
                    return false;
                }
            }
            return CreateDir(szDir);
        }

        public static bool DeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
                return true;
            }
            catch (ArgumentNullException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (NotSupportedException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (IOException)
            {
            }
            return false;
        }

        public static string DirectoryOf(string fullPathName)
        {
            if (fullPathName == null)
            {
                throw new ArgumentNullException("fullPathName");
            }
            FileInfo info = new FileInfo(fullPathName);
            if (info.Exists)
            {
                return info.DirectoryName;
            }
            int length = Math.Max(fullPathName.LastIndexOf('/'), fullPathName.LastIndexOf('\\'));
            if (length > 0)
            {
                return fullPathName.Substring(0, length);
            }
            return "";
        }

        public static string ExtNameOf(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            int num = fileName.LastIndexOf('/');
            int num2 = fileName.LastIndexOf('\\');
            int startIndex = fileName.LastIndexOf('.');
            if (((startIndex > 0) && (startIndex > num)) && (startIndex > num2))
            {
                return fileName.Substring(startIndex);
            }
            return "";
        }

        public static string FileMainNameOf(string fullPathName)
        {
            string str = FileNameOf(fullPathName);
            int length = str.LastIndexOf('.');
            if (length > 0)
            {
                return str.Substring(0, length);
            }
            return str;
        }

        public static string FileNameOf(string fullPathName)
        {
            if (fullPathName == null)
            {
                throw new ArgumentNullException("fullPathName");
            }
            FileInfo info = new FileInfo(fullPathName);
            if (info.Exists)
            {
                return info.Name;
            }
            int num = Math.Max(fullPathName.LastIndexOf('/'), fullPathName.LastIndexOf('\\'));
            if (num > 0)
            {
                return fullPathName.Substring(num + 1);
            }
            return fullPathName;
        }

        public static long FileSizeOf(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            return info.Length;
        }

        public static string[] GetAllFiles(string szDirName, string szFilter, bool bIncludeSubDir)
        {
            DirectoryInfo d = new DirectoryInfo(szDirName);
            List<string> fileNameArr = new List<string>();
            LoadSubDirFileNames(d, szFilter, fileNameArr, bIncludeSubDir);
            return fileNameArr.ToArray();
        }

        public static string[] GetAllSubDir(string szDirName, bool bRescure)
        {
            List<string> list = new List<string>();
            foreach (string str in Directory.GetDirectories(szDirName))
            {
                list.Add(str);
                if (bRescure)
                {
                    list.AddRange(GetAllSubDir(str, true));
                }
            }
            return list.ToArray();
        }

        public static string GetAssembleDirFileName(string fileName)
        {
            return GetFileName(AssembleDirName, fileName);
        }

        public static string GetCurrentDirFileName(string fileName)
        {
            return GetFileName(CurrentDirName, fileName);
        }

        public static string GetExeDirFileName(string fileName)
        {
            return GetFileName(ExeDirName, fileName);
        }

        public static string GetFileName(string fileName)
        {
            int num = fileName.LastIndexOfAny(new char[] { '/', '\\' });
            if (num >= 0)
            {
                return fileName.Substring(num + 1);
            }
            return fileName;
        }

        public static string GetFileName(string dirName, string fileName)
        {
            if (dirName == null)
            {
                throw new ArgumentNullException("dirName");
            }
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (!dirName.EndsWith(@"\") && !dirName.EndsWith("/"))
            {
                return (dirName + @"\" + fileName);
            }
            return (dirName + fileName);
        }

        public static long GetFileSize(string fileFullName)
        {
            try
            {
                FileInfo info = new FileInfo(fileFullName);
                return info.Length;
            }
            catch (Exception)
            {
                return -1L;
            }
        }

        public static string GetFullFileName(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            if (!info.Exists)
            {
                return null;
            }
            return info.FullName;
        }

        public static string GetFullFileName(string fileName, string[] searchDirs)
        {
            for (int i = 0; i < searchDirs.Length; i++)
            {
                string fullFileName = GetFullFileName(searchDirs[i] + @"\" + fileName);
                if (fullFileName != null)
                {
                    return fullFileName;
                }
            }
            return null;
        }

        public static string GetFullPath(string fileName)
        {
            if (fileName.IndexOf(":") < 0)
            {
                fileName = Application.StartupPath + @"\" + fileName;
            }
            FileInfo info = new FileInfo(fileName);
            return info.FullName;
        }

        public static DateTime GetLastWriteTime(string fileFullName)
        {
            try
            {
                FileInfo info = new FileInfo(fileFullName);
                return info.LastWriteTime;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        public static string GetTempFileName()
        {
            return Path.GetTempFileName();
        }

        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        public static bool IsAbsolutePath(string pathName)
        {
            if (pathName == null)
            {
                throw new ArgumentNullException("pathName");
            }
            if (!pathName.StartsWith("/") && !pathName.StartsWith(@"\"))
            {
                return pathName.Contains(":");
            }
            return true;
        }

        public static bool IsDirectory(string dirName)
        {
            if (string.IsNullOrEmpty(dirName))
            {
                return false;
            }
            DirectoryInfo info = new DirectoryInfo(dirName);
            return info.Exists;
        }

        public static bool IsDirExist(string dirName)
        {
            if (IsFileExist(dirName))
            {
                return false;
            }
            return IsDirectory(dirName);
        }

        public static bool IsEmptyDir(string szDir)
        {
            DirectoryInfo info = new DirectoryInfo(szDir);
            if (!info.Exists)
            {
                return false;
            }
            return ((info.GetDirectories().Length <= 0) && (info.GetFiles().Length <= 0));
        }

        public static bool IsFileExist(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            try
            {
                FileInfo info = new FileInfo(fileName);
                return info.Exists;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        private static void LoadSubDirFileNames(DirectoryInfo d, string szFilter, ICollection<string> fileNameArr, bool bIncludeSubDir)
        {
            try
            {
                foreach (FileInfo info in d.GetFiles(szFilter))
                {
                    fileNameArr.Add(info.FullName);
                }
            }
            catch (DirectoryNotFoundException)
            {
                return;
            }
            if (bIncludeSubDir)
            {
                foreach (DirectoryInfo info2 in d.GetDirectories())
                {
                    LoadSubDirFileNames(info2, szFilter, fileNameArr, true);
                }
            }
        }

        public static string LookFor(string[] fileNames)
        {
            foreach (string str in fileNames)
            {
                if (IsFileExist(str))
                {
                    return str;
                }
            }
            return null;
        }

        public static string LookFor(string[] dirNames, string fileName)
        {
            foreach (string str in dirNames)
            {
                string str2 = GetFileName(str, fileName);
                if (IsFileExist(str2))
                {
                    return str2;
                }
            }
            return null;
        }

        public static bool MoveFile(string fileName, string destDir)
        {
            return MoveFile(fileName, destDir, false);
        }

        public static bool MoveFile(string fileName, string destDir, bool overwrite)
        {
            try
            {
                string str = GetFileName(destDir, FileNameOf(fileName));
                if (overwrite && IsFileExist(str))
                {
                    DeleteFile(str);
                }
                File.Move(fileName, str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool MoveFileTo(string fileName, string destDir)
        {
            return MoveFileTo(fileName, destDir, true);
        }

        public static bool MoveFileTo(string fileName, string destDir, bool overwrite)
        {
            if (!string.Equals(DirectoryOf(fileName).ToUpper(), destDir.ToUpper()))
            {
                string str = GetFileName(destDir, FileNameOf(fileName));
                if (IsFileExist(str))
                {
                    if (!overwrite)
                    {
                        return false;
                    }
                    DeleteFile(str);
                }
                else
                {
                    CreateDir2(destDir, true);
                }
                File.Move(fileName, str);
            }
            return true;
        }

        public static byte[] ReadDataFromFile(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        public static string ReadFromTxtFile(string fileName)
        {
            return ReadFromTxtFile(fileName, "GBK");
        }

        public static string ReadFromTxtFile(string fileName, string codeName)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                return File.ReadAllText(fileName);
            }
            return File.ReadAllText(fileName, Encoding.GetEncoding(codeName));
        }

        public static string[] ReadLinesFromTxtFile(string fileName)
        {
            return ReadLinesFromTxtFile(fileName, "GBK");
        }

        public static string[] ReadLinesFromTxtFile(string fileName, string codeName)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                return File.ReadAllLines(fileName);
            }
            return File.ReadAllLines(fileName, Encoding.GetEncoding(codeName));
        }

        public static string[] ReadLinesFromTxtFile(string fileName, int firstLine, int count)
        {
            return ReadLinesFromTxtFile(fileName, firstLine, count, "GBK");
        }

        public static string[] ReadLinesFromTxtFile(string fileName, int firstLine, int count, string codeName)
        {
            if (firstLine < 0)
            {
                throw new ArgumentException("第一行必须为非负数", "firstLine");
            }
            if (count < 0)
            {
                throw new ArgumentException("count必须为非负数", "count");
            }
            if (count == 0)
            {
                return new string[0];
            }
            List<string> list = new List<string>();
            int num = 0;
            using (StreamReader reader = new StreamReader(fileName, Encoding.GetEncoding(codeName)))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    if (count <= 0)
                    {
                        goto Label_007A;
                    }
                    if (num >= firstLine)
                    {
                        list.Add(str);
                        count--;
                        if (count == 0)
                        {
                            goto Label_007A;
                        }
                    }
                    num++;
                }
            }
        Label_007A:
            return list.ToArray();
        }

        public static void RmDir(string dirName)
        {
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.wFunc = 3;
            lpFileOp.pFrom = dirName + "\0";
            SHFileOperation(ref lpFileOp);
        }

        public static void SetFileModifyTime(string destFileName, DateTime dt)
        {
            File.SetLastWriteTime(destFileName, dt);
        }

        public static bool SetFileReadonly(string fileName, bool bReadonly)
        {
            try
            {
                FileAttributes fileAttributes = bReadonly ? (File.GetAttributes(fileName) | FileAttributes.ReadOnly) : FileAttributes.Normal;
                File.SetAttributes(fileName, fileAttributes);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("shell32.dll", EntryPoint="SHFileOperationA")]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);
        public static void WriteDataToFile(string fileName, byte[] data)
        {
            File.WriteAllBytes(fileName, data);
        }

        public static void WriteDataToFile(string fileName, byte[] data, int offset, int count)
        {
            FileStream stream = File.Create(fileName, 0x2800);
            stream.SetLength(0L);
            stream.Write(data, offset, count);
        }

        public static void WriteLinesToTxtFile(string fileName, string[] s)
        {
            WriteLinesToTxtFile(fileName, s, "GBK");
        }

        public static void WriteLinesToTxtFile(string fileName, string[] s, string codeName)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                File.WriteAllLines(fileName, s);
            }
            else
            {
                File.WriteAllLines(fileName, s, Encoding.GetEncoding(codeName));
            }
        }

        public static void WriteToTxtFile(string fileName, string s)
        {
            WriteToTxtFile(fileName, s, "GBK");
        }

        public static void WriteToTxtFile(string fileName, string s, string codeName)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                File.WriteAllText(fileName, s);
            }
            else
            {
                File.WriteAllText(fileName, s, Encoding.GetEncoding(codeName));
            }
        }

        public static string AssembleDirName
        {
            get
            {
                Uri uri = new Uri(Assembly.GetCallingAssembly().CodeBase);
                return DirectoryOf(uri.LocalPath);
            }
        }

        public static string AssembleName
        {
            get
            {
                Uri uri = new Uri(Assembly.GetCallingAssembly().CodeBase);
                return uri.LocalPath;
            }
        }

        public static string CurrentDirName
        {
            get
            {
                return Directory.GetCurrentDirectory();
            }
        }

        public static string ExeDirName
        {
            get
            {
                return DirectoryOf(Application.ExecutablePath);
            }
        }

        public static string ExeName
        {
            get
            {
                return Application.ExecutablePath;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEOPSTRUCT
        {
            internal int hWnd;
            internal int wFunc;
            internal string pFrom;
            internal string pTo;
            internal short fFlags;
            internal int fAborted;
            internal int hNameMaps;
            internal string sProgress;
        }
    }
}

