using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace AssetBundleFramework
{
    /*
      自动设置ab包信息
     */
    public class AutoAssetBundleInfo
    {
        [MenuItem("Tools/AssetBundle/SetAesstBundleInfo")]
        static private void SetAesstBundleInfo()
        {
            //清除无用的AB标志
            AssetDatabase.RemoveUnusedAssetBundleNames();
            //需要打AB包的资源路径
            string _needToAbRescourcePath = AssetBundlePathTools.GetABRescourcePath();
            //跟目录
            DirectoryInfo _needToAbRootDir = new DirectoryInfo(_needToAbRescourcePath);
            //文件夹类型
            DirectoryInfo[] _typeDirs = _needToAbRootDir.GetDirectories();

            foreach (DirectoryInfo dir in _typeDirs)
            {
                //获取全路径
                string _tmpDirPath = _needToAbRescourcePath + "/" + dir.Name;
                int _index = _tmpDirPath.LastIndexOf("/");
                //类型文件夹名字
                string _typeName = _tmpDirPath.Substring(_index + 1);
                //Debug.Log(_typeName);
                ForeachDir(dir, _typeName);

            }
            AssetDatabase.Refresh();
            Log.LogColor("[ AssetBundle信息设置完成 ]", Color.green);
        }

        /// <summary>
        /// 遍历文件夹获取该文件夹内全部文件,直到非文件夹为止
        /// </summary>

        static private void ForeachDir(FileSystemInfo pFileInfo, string pTypeName)
        {
            if (pFileInfo.Exists == false)
            {
                return;
            }

            FileSystemInfo[] _fileInofs = (pFileInfo as DirectoryInfo).GetFileSystemInfos();
            foreach (FileSystemInfo file in _fileInofs)
            {
                FileInfo _fileInfo = file as FileInfo;
                //如果有文件信息则是资源文件，设置资源信息
                if (_fileInfo != null)
                {
                    SetAssetBundleInfo(_fileInfo, pTypeName);
                }
                //否则目录，递归
                else
                {
                    ForeachDir(file, pTypeName);
                }
            }

        }

        static private void SetAssetBundleInfo(FileInfo pFile, string pTypeName)
        {
            //过滤文件
            if (pFile.Extension == ".meta")
                return;

            string _abName = GetRescourceFileName(pFile, pTypeName);
            int _index = pFile.FullName.IndexOf("Assets");
            //资源相对路径
            string _assetFilePath = pFile.FullName.Substring(_index);
            AssetImporter _assetImp = AssetImporter.GetAtPath(_assetFilePath);
            _assetImp.assetBundleName = _abName;
            //资源分类
            if (pFile.Extension == ".unity")
            {
                //场景
                _assetImp.assetBundleVariant = "unity3d";
            }
            else
            {
                //资源
                _assetImp.assetBundleVariant = "ab";
            }

        }

        /// <summary>
        /// 获取资源名字
        /// </summary>
        static private string GetRescourceFileName(FileInfo pFile, string pTypeName)
        {
            string _unityFilePath = pFile.FullName.Replace("\\", "/");
            //Debug.Log(_unityFilePath);
            int _index = _unityFilePath.IndexOf(pTypeName) + pTypeName.Length;
            string _fileName = _unityFilePath.Substring(_index + 1);
            //Debug.Log(_fileName);
            if (_fileName.Contains("/"))
            {
                string[] _tmpStrs = _fileName.Split('/');
                _fileName = pTypeName + "/" + _tmpStrs[0];
            }
            else
            {
                _fileName = pTypeName + "/" + pTypeName;
            }

            return _fileName;
        }


        static private void LogMsg(object pMsg)
        {
            string _msg = "<color=#1DFF00>" + pMsg + "</color>";
            Debug.Log(_msg);
        }
    }
}


