using System.Text;
using System.IO;
using System.Security.Cryptography;
using System;

namespace Helper
{
    public delegate void FileDelegate();
    public delegate void FileDelegateCallBack();
    public class FileHelper
    {
        static public string GetFileMd5(string pFilePath)
        {
            StringBuilder _sb = new StringBuilder();
            using (FileStream _fs = new FileStream(pFilePath, FileMode.Open))
            {
                MD5 _md5 = new MD5CryptoServiceProvider();
                byte[] _buffer = _md5.ComputeHash(_fs);
                for (int i = 0; i < _buffer.Length; i++)
                {
                    _sb.Append(_buffer[i].ToString("x2"));
                }
            }
            return _sb.ToString();
        }
        
        /// <summary>
        /// 异步写入文件数据
        /// </summary>
        static public void AsyncWriteFileData(string pPath,string pContent, Action pCallBack)
        {
            FileDelegate _dele = new FileDelegate(delegate { WriteFile(pPath, pContent, pCallBack); });
            IAsyncResult _result = _dele.BeginInvoke(null, null);
        } 

        static public void WriteFile(string pPath, string pContent, Action pCallBack)
        {
    
            File.AppendAllText(pPath, pContent); 
            pCallBack?.Invoke();
        }
    }
}

