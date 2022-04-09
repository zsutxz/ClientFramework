using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace UIFramework
{

    public class UIHelper
    {

        static public void AddButtonListenser(Button pBtn, UnityAction pDele)
        {
            if (pBtn == null)
                return;
            pBtn.onClick.AddListener(pDele);
        }

        static public void RemoveButtonListenser(Button pBtn, UnityAction pDele)
        {
            if (pBtn == null)
                return;
      
            pBtn.onClick.RemoveListener(pDele);
        }

        static public void RemoveAllButtonListenser(Button pBtn)
        {
            if (pBtn == null)
                return;
            pBtn.onClick.RemoveAllListeners();
        }
    }
}

