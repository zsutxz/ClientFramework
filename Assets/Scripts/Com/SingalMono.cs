using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingalMono<T> : MonoBehaviour where T: SingalMono<T>
{
    static private T m_instance;
    static public T Instance 
    {
        get
        {
            if (m_instance == null)
            {
                GameObject _newGo = new GameObject(typeof(T).ToString());
                _newGo.hideFlags = HideFlags.HideInHierarchy;
                m_instance = _newGo.AddComponent<T>();
                DontDestroyOnLoad(_newGo);
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        InitMono();
    }

    protected virtual void InitMono() { }
}
