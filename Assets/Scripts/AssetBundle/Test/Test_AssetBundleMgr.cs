using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundleFramework;
public class Test_AssetBundleMgr : MonoBehaviour
{
    // Start is called before the first frame update
    AssetPackageInfo _info;
    AssetPackageInfo _info2;
    void Start()
    {
         _info = new AssetPackageInfo();
        _info.moduleName = "model";
        _info.packageName = "model/model.ab";
        _info.assetName = "Cube.prefab";

        _info2 = new AssetPackageInfo();
        _info2.moduleName = "model2";
        _info2.packageName = "model2/model2.ab";
        _info2.assetName = "Sphere.prefab";


        AssetBundleMgr.Instance.LoadAssetPackage(_info, CallBack);
        AssetBundleMgr.Instance.LoadAssetPackage(_info2, CallBack2);
    }

    private void CallBack(string pName)
    {
        Object _obj = AssetBundleMgr.Instance.LoadAsset(_info);
        for (int i = 0; i < 10; i++)
        {
            Instantiate(_obj);
        }
     
    }
    private void CallBack2(string pName)
    {
        Object _obj = AssetBundleMgr.Instance.LoadAsset(_info2);
        for (int i = 0; i < 10; i++)
        {
            Instantiate(_obj);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            AssetBundleMgr.Instance.DisposeModuleAssets(_info);
        }
        if (Input.GetKeyDown("q"))
        {
            AssetBundleMgr.Instance.DisposeAllAsset();
        }
    }
}
