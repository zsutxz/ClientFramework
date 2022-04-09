using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
/*
 �����ļ���StreamingAssets�ļ���
 */
public partial class FileEditor
{


    [MenuItem("Tools/Lua/CopyLuaToStreamingAssets")]
    static private void CopyLuaFileToSA()
    {
        //�༭��lua�ļ�·��
        string _luaDirPath = Application.dataPath + HotUpdate.HotUpdatePathTools.LuaEditorPath;
        //������lua·��
        string _luaToDirPath = HotUpdate.HotUpdatePathTools.GetDeloyPath() + HotUpdate.HotUpdatePathTools.LuaDelpoyPath;
        DirectoryInfo _luaDir = new DirectoryInfo(_luaDirPath);
        FileInfo[] _filesInfo = _luaDir.GetFiles();

        if (Directory.Exists(_luaToDirPath))
        {
            Directory.Delete(_luaToDirPath, true);
        }
        Directory.CreateDirectory(_luaToDirPath);

        CopyLua(_luaDir, _luaToDirPath);
        AssetDatabase.Refresh();
        Log.LogColor("[lua�ļ�copy��Ŀ��Ŀ¼���]", Color.green);
    }

    [MenuItem("Tools/Lua/CopyLuaToABRescource")]
    static private void CopyLuaFileToR()
    {
        //�༭��lua�ļ�·��
        string _luaDirPath = Application.dataPath + HotUpdate.HotUpdatePathTools.LuaEditorPath;
        //������lua·��
        string _luaToDirPath = HotUpdate.HotUpdatePathTools.GetABRescourcePath() + HotUpdate.HotUpdatePathTools.ABResourceLuaPath;
        DirectoryInfo _luaDir = new DirectoryInfo(_luaDirPath);
        FileInfo[] _filesInfo = _luaDir.GetFiles();

        if (Directory.Exists(_luaToDirPath))
        {
            Directory.Delete(_luaToDirPath,true);
        }
        Directory.CreateDirectory(_luaToDirPath);

        CopyLua(_luaDir, _luaToDirPath);
        AssetDatabase.Refresh();
        Log.LogColor("[lua�ļ�copy��Ŀ��Ŀ¼���]", Color.green);
    }


    static private void CopyLua(FileSystemInfo pFileSysInfo, string pCopyPath)
    {
        if (pFileSysInfo.Exists == false)
            return;
        FileSystemInfo[] _fileSystemInfos = (pFileSysInfo as DirectoryInfo).GetFileSystemInfos();
        foreach (FileSystemInfo file in _fileSystemInfos)
        {
            FileInfo _info = file as FileInfo;
            if (_info != null)
            {
                //Debug.Log(file.);
                if (_info.Extension == ".lua" || _info.Extension == ".Lua" || _info.Extension == ".LUA")
                {
                    int _index = file.Name.IndexOf('.');
                    string _newName = file.Name.Substring(0, _index);
                    _newName += ".txt";
                    File.Copy(file.FullName, pCopyPath + "/" + _newName, true);
                }

            }
            else
            {
                CopyLua(file, pCopyPath);
            }
        }

    }

}
public partial class FileEditor
{

    [MenuItem("Tools/Config/CopyConfigToStreamingAssets")]
    static private void CopyConfigFile()
    {
        //�༭�������ļ�·��
        string _ConfigEditorPath = Application.dataPath + HotUpdate.HotUpdatePathTools.ConfigEditorPath + "/";
        //�����������ļ�·��
        string _ConfigDelopyPath = HotUpdate.HotUpdatePathTools.GetDeloyPath() + HotUpdate.HotUpdatePathTools.ConfigDeployPath + "/";
        ////�����ļ�Ŀ¼

        if (!Directory.Exists(_ConfigDelopyPath))
            Directory.CreateDirectory(_ConfigDelopyPath);

        CoyeFile(_ConfigEditorPath, _ConfigDelopyPath);

        AssetDatabase.Refresh();
        Log.LogColor("[Config�ļ�copy��Ŀ��Ŀ¼���]", Color.green);
    }

    static private void CoyeFile(string pFromPath, string pToPath)
    {
        if (!Directory.Exists(pToPath))
            Directory.CreateDirectory(pToPath);
        string[] _fileArr = Directory.GetFileSystemEntries(pFromPath);
        foreach (string file in _fileArr)
        {
            string _tmpFile = file.Replace("\\", "/");
            string _tmpPath = pToPath + "/" + Path.GetFileName(file);
            if (Directory.Exists(file))
            {
                // �ݹ�
                CoyeFile(_tmpFile, _tmpPath);
            }
            else
            {
                //�����ļ�
                if (file.EndsWith(".meta"))
                    continue;
                File.Copy(_tmpFile, _tmpPath, true);
            }
        }
    }
}

public partial class FileEditor
{


    [MenuItem("Tools/Verify/CreateVerifyFile")]
    static private void CreateVerifyFile()
    {
        string _savePath = HotUpdate.HotUpdatePathTools.GetDeloyPath();
        string _verifyFilePath = _savePath + HotUpdate.HotUpdatePathTools.VerifyFile;
        if (File.Exists(_verifyFilePath))
            File.Delete(_verifyFilePath);

        List<string> _md5FileList = new List<string>();
        DirectoryInfo _dir = new DirectoryInfo(_savePath);
        NeedToMD5FileList(_dir, ref _md5FileList);
        WriteVerity(_verifyFilePath, _savePath, _md5FileList);
        AssetDatabase.Refresh();
        Log.LogColor("[�����ļ������ɹ�]", Color.green);
    }

    /// <summary>
    /// ��ȡ��Ҫ����MD5���ļ�·��
    /// </summary>
    /// <param name="pFileSys"></param>
    /// <param name="pFileList"></param>
    static private void NeedToMD5FileList(FileSystemInfo pFileSys, ref List<string> pFileList)
    {
        FileSystemInfo[] _fileSysInfos = (pFileSys as DirectoryInfo).GetFileSystemInfos();
        foreach (FileSystemInfo fileSys in _fileSysInfos)
        {
            FileInfo _file = fileSys as FileInfo;
            if (_file != null)
            {
                string _fileFullName = _file.FullName.Replace("\\", "/");
                //�����ļ�
                string _fileExtension = Path.GetExtension(_fileFullName);
                if (_fileExtension.EndsWith(".meta"))
                    continue;
                else if (_fileExtension.EndsWith(".bak"))
                    continue;
                else
                    pFileList.Add(_fileFullName);
            }
            else
            {
                NeedToMD5FileList(fileSys, ref pFileList);
            }
        }
    }

    static private void WriteVerity(string pVeritySavePath, string pToPath, List<string> pFilePathList)
    {
        using (FileStream _fs = new FileStream(pVeritySavePath, FileMode.CreateNew))
        {
            using (StreamWriter _sw = new StreamWriter(_fs))
            {
                foreach (string file in pFilePathList)
                {
                    string _fileMd5 = Helper.FileHelper.GetFileMd5(file);
                    string _fileName = file.Replace(pToPath + "/", string.Empty);
                    _sw.WriteLine(_fileName + "|" + _fileMd5);
                }
            }
        }
    }

}



