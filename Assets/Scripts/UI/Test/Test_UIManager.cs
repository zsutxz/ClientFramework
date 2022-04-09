using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using System;

public class Test_UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      //  UIManager.Instance.LoadCanvas(OnFinishCallBack);
    }

    private void OnFinishCallBack(GameObject pCanvas)
    {

        UIManager.Instance.OpenForm("UILoginForm");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            UIManager.Instance.CloseForm("UILoginForm");
        }   
    }

    public void Open1()
    {
        UIManager.Instance.OpenForm("UIPopForm");
        UIManager.Instance.CloseForm("UILoginForm");
    }
    public void Open2()
    {
        UIManager.Instance.OpenForm("UILoginForm");
        UIManager.Instance.CloseForm("UIPopForm");
    }

}
