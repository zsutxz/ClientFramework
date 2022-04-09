using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace AssetBundleFramework
{
    /*
      �Զ�����ab����Ϣ
     */
    public class AutoAssetBundleInfo
    {
        [MenuItem("Tools/AssetBundle/SetAesstBundleInfo")]
        static private void SetAesstBundleInfo()
        {
            //������õ�AB��־
            AssetDatabase.RemoveUnusedAssetBundleNames();
            //��Ҫ��AB������Դ·��
            string _needToAbRescourcePath = AssetBundlePathTools.GetABRescourcePath();
            //��Ŀ¼
            DirectoryInfo _needToAbRootDir = new DirectoryInfo(_needToAbRescourcePath);
            //�ļ�������
            DirectoryInfo[] _typeDirs = _needToAbRootDir.GetDirectories();

            foreach (DirectoryInfo dir in _typeDirs)
            {
                //��ȡȫ·��
                string _tmpDirPath = _needToAbRescourcePath + "/" + dir.Name;
                int _index = _tmpDirPath.LastIndexOf("/");
                //�����ļ�������
                string _typeName = _tmpDirPath.Substring(_index + 1);
                //Debug.Log(_typeName);
                ForeachDir(dir, _typeName);

            }
            AssetDatabase.Refresh();
            Log.LogColor("[ AssetBundle��Ϣ������� ]", Color.green);
        }

        /// <summary>
        /// �����ļ��л�ȡ���ļ�����ȫ���ļ�,ֱ�����ļ���Ϊֹ
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
                //������ļ���Ϣ������Դ�ļ���������Դ��Ϣ
                if (_fileInfo != null)
                {
                    SetAssetBundleInfo(_fileInfo, pTypeName);
                }
                //����Ŀ¼���ݹ�
                else
                {
                    ForeachDir(file, pTypeName);
                }
            }

        }

        static private void SetAssetBundleInfo(FileInfo pFile, string pTypeName)
        {
            //�����ļ�
            if (pFile.Extension == ".meta")
                return;

            string _abName = GetRescourceFileName(pFile, pTypeName);
            int _index = pFile.FullName.IndexOf("Assets");
            //��Դ���·��
            string _assetFilePath = pFile.FullName.Substring(_index);
            AssetImporter _assetImp = AssetImporter.GetAtPath(_assetFilePath);
            _assetImp.assetBundleName = _abName;
            //��Դ����
            if (pFile.Extension == ".unity")
            {
                //����
                _assetImp.assetBundleVariant = "unity3d";
            }
            else
            {
                //��Դ
                _assetImp.assetBundleVariant = "ab";
            }

        }

        /// <summary>
        /// ��ȡ��Դ����
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


