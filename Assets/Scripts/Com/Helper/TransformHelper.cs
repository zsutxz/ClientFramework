using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public class TransformHelper
    {
        static public void AddComponent(GameObject pObj, string pType)
        {
            pObj.AddComponent(Type.GetType(pType));
        }

        static public Transform GetChild(Transform pTf, string pChildName)
        {
            if (pTf == null)
                return null;
            if (string.IsNullOrEmpty(pChildName))
                return null;
            //�ݹ飺�����ڲ���������Ĺ��̡�

            //����������
            Transform childTF = pTf.Find(pChildName);
            if (childTF != null) return childTF;
            //����������
            for (int i = 0; i < pTf.childCount; i++)
            {
                childTF = GetChild(pTf.GetChild(i), pChildName);
                if (childTF != null) return childTF;
            }
            return null;
        }

    }
}

