using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMARTII.Domain.IO
{
    public static class FileUtility
    {
        /// <summary>
        /// 建立目錄
        /// </summary>
        /// <param name="dirpath"></param>
        public static void CreateDirectory(string dirpath)
        {
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
        }

        /// <summary>
        /// 取得檔案文字
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileText(string fullPath)
        {
            return File.ReadAllText(fullPath);
        }

        /// <summary>
        /// 取得檔案byte
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] GetFileBytes(string fullPath)
        {
            return File.ReadAllBytes(fullPath);
        }

        /// <summary>
        /// 取得檔案文字
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] GetFileTexts(string dirpath, string fileName = null)
        {
            var result = new List<string>();

            var files = Directory.GetFiles(dirpath);

            var current = string.IsNullOrEmpty(fileName) ?
                files : files.Where(x => x.Contains(Path.GetFileName(fileName)));

            foreach (var file in current)
            {
                var txt = File.ReadAllText(file);

                result.Add(txt);
            }

            return result.ToArray();
        }

        /// <summary>
        /// 取得檔案byte
        /// </summary>
        /// <param name="dirpath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[][] GetFileBytes(string dirpath, string fileName = null)
        {
            var result = new List<byte[]>();

            var files = Directory.GetFiles(dirpath);

            var current = string.IsNullOrEmpty(fileName) ?
                files : files.Where(x => x.Contains(Path.GetFileName(fileName)));

            foreach (var file in current)
            {
                var txt = File.ReadAllBytes(file);

                result.Add(txt);
            }

            return result.ToArray();
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="fullPath"></param>
        public static void DeleteFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="fullPath"></param>
        public static void DeleteFiles(string[] fullPaths)
        {
            foreach (var path in fullPaths)
            {
                DeleteFile(path);
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="dirPath"></param>
        public static void DeleteDirectory(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
            }
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static void SaveFile(this byte[] bytes, string savePath, string fileName = "")
        {
            string fullPath = Path.Combine(savePath, fileName);
            (new FileInfo(fullPath)).Directory.Create();
            File.WriteAllBytes(fullPath, bytes);
        }

        /// <summary>
        /// 儲存檔案 , 並返回相對路徑
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="filePathPrefix"></param>
        /// <returns></returns>
        public static string SaveAsFilePath(this byte[] bytes,
                                                 string path,
                                                 string fileName = "")
        {
            bytes.SaveFile(path, fileName);

            return $"{path}{fileName}";
        }

        /// <summary>
        /// Convert Stream to byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(this Stream stream)

        {
            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 設定當前流的位置為流的開始
            stream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }
    }
}