using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Test_HotFix : MonoBehaviour
{
    private Image m_image;
    private void Start()
    {
        m_image = GetComponent<Image>();
        Init();
        
    }
  
    private void Init()
    {
        m_image.color = Color.white;
    }
}
